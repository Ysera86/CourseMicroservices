using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        /// <summary>
        /// CancellationToken, foto diyelim 30 snde kaydoluyor, 10. saniyede kullanıcı tarayıcıyı kapatırsa otomatik olarak CancellationToken tetiklenir ve bu işlemi durdurur. Bu olmazsa asenkron da olduğu için arkada bir süre daha devam ederdi.
        /// Asenkron bir işlemi ancak hata fırlatarak sonlandırabiliriz -CancellationToken da hata fırlatarak işlemi sonlandırıyor.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PhotosSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                // random isim oluşturmak daha iyi olurdu
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                // http://www.photostock.api.com/photos.asdljasıda.jpg
                var returnPath = $"photos/{photo.FileName}";

                PhotoDto photoDto = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, StatusCodes.Status200OK));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty", StatusCodes.Status400BadRequest));
        }

        [HttpDelete]
        public IActionResult Delete(string photoName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoName);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found", StatusCodes.Status404NotFound));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(StatusCodes.Status204NoContent));
        }
    }
}
