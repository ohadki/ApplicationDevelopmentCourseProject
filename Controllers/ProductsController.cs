using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Cryptography;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    //TODO: HANDLE CRUD VIEW RETURNS

    public class ProductsController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;
        private readonly string _imagesFolderPath;
        //TODO: move to the config file
        private readonly string _TwitterTextAPI = "https://api.twitter.com/1.1/statuses/update.json";
        private readonly string _TwitterImageAPI = "https://upload.twitter.com/1.1/media/upload.json";

        private readonly string _consumerKey;
        private static readonly string _consumerKeySecret;
        private readonly string _accessToken ;
        private static readonly string _accessTokenSecret;
        private readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly HMACSHA1 _sigHasher = new HMACSHA1(
            new ASCIIEncoding().GetBytes($"{_consumerKeySecret}&{_accessTokenSecret}"));

        [Obsolete]
        public ProductsController(ApplicationDevelopmentCourseProjectContext context, IHostingEnvironment _environment)
        {
            _context = context;
            _imagesFolderPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\UploadFiles\\Products";
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UploadProduct()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category.ToList(), nameof(Category.Id), nameof(Category.Name));
            return View();
        }

    // GET: Products
    public async Task<IActionResult> Index()
        {
            var applicationDevelopmentCourseProjectContext = _context.Product.Include(p => p.Category);
            return View(await applicationDevelopmentCourseProjectContext.ToListAsync());
        }

        // GET: Products1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products1/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            return View();
        }

        // POST: Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {  
            if(HttpContext.Request.Form.Files.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    IFormFile productImage = HttpContext.Request.Form.Files[0];
                    string ext = Path.GetExtension(productImage.FileName);
                    product.Image = Guid.NewGuid() + ext; // random name
                    using (Stream fileStream = new FileStream(Path.Combine(_imagesFolderPath, product.Image), FileMode.Create))
                    {
                        productImage.CopyTo(fileStream);
                    }
                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    PublishToTwitter("The product" + product.Name + "uploaded to the shop in category + "
                        + product.Category + " with the the price " + product.Price, product.Image);

                    return RedirectToAction(nameof(Index),"Home");
                }
            }
            return View(nameof(Index), "Home");
        }

        // GET: Products1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // POST: Products1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        /// <summary>
        /// Publish a post with image
        /// </summary>
        /// <returns>result</returns>
        /// <param name="post">post to publish</param>
        /// <param name="pathToImage">image to attach</param>
        public string PublishToTwitter(string post, string pathToImage)
        {
            try
            {
                // first, upload the image
                string mediaID = string.Empty;
                var rezImage = Task.Run(async () =>
                {
                    var response = await TweetImage(pathToImage);
                    return response;
                });
                var rezImageJson = JObject.Parse(rezImage.Result.Item2);

                if (rezImage.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error uploading image to Twitter. {rezImageJson["errors"][0]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return "Unknown error uploading image to Twitter";
                    }
                }
                mediaID = rezImageJson["media_id_string"].Value<string>();

                // second, send the text with the uploaded image
                var rezText = Task.Run(async () =>
                {
                    var response = await TweetText(post, mediaID);
                    return response;
                });
                var rezTextJson = JObject.Parse(rezText.Result.Item2);

                if (rezText.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error sending post to Twitter. {rezTextJson["errors"][0]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return "Unknown error sending post to Twitter";
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                // log exception somewhere
                return "Unknown error publishing to Twitter";
            }
        }

        /// <summary>
        /// Send a tweet with some image attached
        /// </summary>
        /// <returns>HTTP StatusCode and response</returns>
        /// <param name="text">Text</param>
        /// <param name="mediaID">Media ID for the uploaded image. Pass empty string, if you want to send just text</param>
        public Task<Tuple<int, string>> TweetText(string text, string mediaID)
        {
            var textData = new Dictionary<string, string> {
                { "status", text },
                { "trim_user", "1" },
                { "media_ids", mediaID}
            };

            return SendText(_TwitterTextAPI, textData);
        }

        /// <summary>
        /// Upload some image to Twitter
        /// </summary>
        /// <returns>HTTP StatusCode and response</returns>
        /// <param name="pathToImage">Path to the image to send</param>
        public Task<Tuple<int, string>> TweetImage(string pathToImage)
        {
            byte[] imgdata = System.IO.File.ReadAllBytes(pathToImage);
            var imageContent = new ByteArrayContent(imgdata);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(imageContent, "media");

            return SendImage(_TwitterImageAPI, multipartContent);
        }

        async Task<Tuple<int, string>> SendText(string URL, Dictionary<string, string> textData)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(URL, textData, "POST"));

                var httpResponse = await httpClient.PostAsync(URL, new FormUrlEncodedContent(textData));
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }

        async Task<Tuple<int, string>> SendImage(string URL, MultipartFormDataContent multipartContent)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(URL, null, "POST"));

                var httpResponse = await httpClient.PostAsync(URL, multipartContent);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }

        #region Some OAuth magic
        string PrepareOAuth(string URL, Dictionary<string, string> data, string httpMethod)
        {
            // seconds passed since 1/1/1970
            var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

            // Add all the OAuth headers we'll need to use when constructing the hash
            Dictionary<string, string> oAuthData = new Dictionary<string, string>();
            oAuthData.Add("oauth_consumer_key", _consumerKey);
            oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
            oAuthData.Add("oauth_timestamp", timestamp.ToString());
            oAuthData.Add("oauth_nonce", Guid.NewGuid().ToString());
            oAuthData.Add("oauth_token", _accessToken);
            oAuthData.Add("oauth_version", "1.0");

            if (data != null) // add text data too, because it is a part of the signature
            {
                foreach (var item in data)
                {
                    oAuthData.Add(item.Key, item.Value);
                }
            }

            // Generate the OAuth signature and add it to our payload
            oAuthData.Add("oauth_signature", GenerateSignature(URL, oAuthData, httpMethod));

            // Build the OAuth HTTP Header from the data
            return GenerateOAuthHeader(oAuthData);
        }

        /// <summary>
        /// Generate an OAuth signature from OAuth header values
        /// </summary>
        string GenerateSignature(string url, Dictionary<string, string> data, string httpMethod)
        {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format("{0}&{1}&{2}",
                httpMethod,
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString()
                )
            );

            return Convert.ToBase64String(
                _sigHasher.ComputeHash(
                    new ASCIIEncoding().GetBytes(fullSigData.ToString())
                )
            );
        }

        /// <summary>
        /// Generate the raw OAuth HTML header from the values (including signature)
        /// </summary>
        string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return string.Format(
                "OAuth {0}",
                string.Join(
                    ", ",
                    data
                        .Where(kvp => kvp.Key.StartsWith("oauth_"))
                        .Select(
                            kvp => string.Format("{0}=\"{1}\"",
                            Uri.EscapeDataString(kvp.Key),
                            Uri.EscapeDataString(kvp.Value)
                            )
                        ).OrderBy(s => s)
                    )
                );
        }
        #endregion
    }
}
