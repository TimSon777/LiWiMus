using ByteSizeLib;

namespace LiWiMus.Core.Models;

public record ImageInfo(ByteSize ByteSize, string Extension, SixLabors.ImageSharp.Image Image);
    
