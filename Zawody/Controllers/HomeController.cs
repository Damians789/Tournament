
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Zawody.Models;
using Zawody.Data;
using Zawody.Models.PlayerViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Zawody.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ILogger<HomeController> _logger;*/
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
  
        public HomeController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            /*_logger = logger;*/
            _context = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            List<ImageModel> images = GetImages();
            return View(images);
        }

        [HttpPost]
        public ActionResult Index(int imageId)
        {
            List<ImageModel> images = GetImages();
            ImageModel image = images.Find(p => p.Id == imageId);
            if (image != null)
            {
                image.IsSelected = true;
                ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(image.Data, 0, image.Data.Length);
            }
            return View(images);
        }

        private List<ImageModel> GetImages()
        {
            string query = "SELECT [Id], [Name], [ContentType], [Data], [IsSelected] FROM [Zawody].[Identity].[Images]";
            List<ImageModel> images = new List<ImageModel>();
            /*string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStrings"].ConnectionString;*/
            /*string xd = _context.Database.GetConnectionString();*/
            var xd = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            string hmm = "Server=DESKTOP-7526Q4N;Database=Zawody;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(hmm))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                       /* while (sdr.Read())
                        {
                            images.Add(new ImageModel
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString(),
                                ContentType = sdr["ContentType"].ToString(),
                                Data = (byte[])sdr["Data"]
                            });
                        }*/
                    }
                    con.Close();
                }

                return images;
            }
        }

        

        public async Task<ActionResult> About()
        {
            List<PlayerTeamGroup> groups = new List<PlayerTeamGroup>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT [Name], COUNT(*) AS 'PlayerCount' FROM [Zawody].[Identity].[Person] AS P JOIN [Zawody].[Identity].[Team] AS T ON [P].[TeamID] = [T].[Id] WHERE [P].[Discriminator] = 'Player' GROUP BY [T].[Name];";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new PlayerTeamGroup { Name = reader.GetString(0), PlayerCount = reader.GetInt32(1) };
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return View(groups);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ContentType,Data")] ImageModel image)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        image.Data = dataStream.ToArray();
                    }
/*                    _context.Add(image);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));*/
                }
                _context.Images.AddAsync(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Create3([Bind("Id,Name,ContentType")] ImageModel model)
        {

            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "PrivateFiles");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));


/*                if (ModelState.IsValid)
                {*/
                    model.Name = uniqueFileName;
                    model.Data = GetByteArrayFromImage(model.MyImage);
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
/*                }*/
            }

            return RedirectToAction("Index", "Home");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }


        private void Create2(ImageModel model)
        {

            var post = new ImageModel() { Name = model.Name,
            ContentType = model.ContentType};

            if (model.MyImage != null)
            {
                post.Data = GetByteArrayFromImage(model.MyImage);
            }
            _context.Images.Add(post);
            _context.SaveChanges();

        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}