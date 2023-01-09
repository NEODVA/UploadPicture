using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using UploadPicture.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Hosting;

namespace UploadPicture.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationContext _context;
        IWebHostEnvironment _appEnviroment;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, IWebHostEnvironment appEnviroment)
        {
            _logger = logger;
            _context = context;
            _appEnviroment = appEnviroment;
        }

        public IActionResult Index()
        {
            return View();
            //_context.Files.ToList());
        }


        public IActionResult Privacy()
        {
            return View();
        }
        //private void Base64ToImage(IFormFile file)
        //{

        //    //kodavorum a base64
        //    string base64 = Convert.ToBase64String(file);
        //    //entityn a sarqum
        //    ImageEntity entity = new ImageEntity(file.Name, file.ContentType, file.Length, base64);
        //    //baza save
        //    _context.Images.Add(entity);
        //    //apakodavorel base64-y
        //    //Image image = Image.FromStream(new MemoryStream(Convert.FromBase64String(entity.ImageData)));

        //}
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            //imya otpravitelya
            string senderName = "Karen";
            //0-100 0-n max sexmum a 100 chi kpnum
            long compresseValue = 20L;


            string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(10).ToArray()).Replace(' ', '-');
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);

            var imagePath = Path.Combine(GetAbsolutePath(), "ProcessedImages", fileName);

            using (var fs = new FileStream(imagePath, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fs);
            }

            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                ImageCodecInfo imageInfo = GetEncoder(ImageFormat.Jpeg);
                Encoder encoder = Encoder.Quality;

                EncoderParameters myParams = new EncoderParameters(1);

                EncoderParameter myParam = new EncoderParameter(encoder, compresseValue);
                myParams.Param[0] = myParam;
                string savePath = $"Images/{senderName}/";

                //useri anunov papka a sarqum
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                bitmap.Save($"{savePath}{compresseValue}.{fileName}", imageInfo, myParams);

            }

            //skzbnakan nkary jnjelu hamar
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            return RedirectToAction("Index");
        }
        private string GetAbsolutePath()
        {
            using IHost host = Host.CreateDefaultBuilder().Build();

            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

            return config["Directory"];
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

    }
}