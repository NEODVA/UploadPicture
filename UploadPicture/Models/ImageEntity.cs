using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UploadPicture.Models
{
    public class ImageEntity
    {
        public ImageEntity(string fileName,string imageType, long length, string imageData)
        {
            ImageData = imageData;
            FileName = fileName;
            Length = length;
            ImageType = imageType;
        }


        [Key]
        public int Id { get; private set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FileName { get; private set; }

        public string ImageType { get; private set; }

        [Column]
        public long Length { get; private set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string ImageData { get; private set; }
    }
}
