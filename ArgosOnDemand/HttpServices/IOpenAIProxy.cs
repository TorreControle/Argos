namespace ArgosOnDemand.HttpServices;

public interface IOpenAIProxy
{
    Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, CancellationToken cancellation = default);

    Task<byte[]> DownloadImage(string url);
}
