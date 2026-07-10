using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ostium11.Extensions
{
    public static class ColorSpaceExtensions
    {
        static readonly Lazy<byte[]> LinearToGammaLut = new(() =>
        {
            var result = new byte[256];
            for (var i = 0; i < 256; i++) result[i] = (byte) (Mathf.LinearToGammaSpace(i / 255f) * 255f);
            return result;
        });

        static readonly Lazy<byte[]> GammaToLinearLut = new(() =>
        {
            var result = new byte[256];
            for (var i = 0; i < 256; i++) result[i] = (byte) (Mathf.GammaToLinearSpace(i / 255f) * 255f);
            return result;
        });

        public static byte[] LinearToGammaJpeg(this byte[] data, int quality = 95)
            => ConvertJpegColorSpace(data, true, quality);

        public static byte[] GammaToLinearJpeg(this byte[] data, int quality = 95)
            => ConvertJpegColorSpace(data, false, quality);

        static byte[] ConvertJpegColorSpace(byte[] data, bool toGamma, int quality = 95)
        {
            if (data == null || data.Length == 0) return data;

            var tempTexture = new Texture2D(2, 2);
            if (!tempTexture.LoadImage(data))
            {
                Object.Destroy(tempTexture);
                return data;
            }
        
            var pixels = tempTexture.GetPixels32();
        
            var lut = toGamma ? LinearToGammaLut.Value : GammaToLinearLut.Value;
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i].r = lut[pixels[i].r];
                pixels[i].g = lut[pixels[i].g];
                pixels[i].b = lut[pixels[i].b];
            }
        
            var resultTexture = new Texture2D(
                tempTexture.width, 
                tempTexture.height, 
                TextureFormat.RGBA32, 
                false
            );
            
            resultTexture.SetPixels32(pixels);
            resultTexture.Apply();
        
            var result = resultTexture.EncodeToJPG(quality);
        
            Object.Destroy(tempTexture);
            Object.Destroy(resultTexture);
        
            return result;
        }
    }
}
