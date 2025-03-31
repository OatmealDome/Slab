using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;

namespace OatmealDome.Slab.S3;

public sealed class SlabS3Service
{
    private readonly SlabS3Configuration _settings;
    private readonly AmazonS3Client _client;
    private readonly TransferUtility _transferUtility;

    public SlabS3Service(IOptions<SlabS3Configuration> settings)
    {
        _settings = settings.Value;
        
        _client = new AmazonS3Client(_settings.AccessKey, _settings.SecretAccessKey, new AmazonS3Config()
        {
            ServiceURL = _settings.ServiceUrl,

            // This is required to work with unofficial S3 implementations like Wasabi.
            // https://github.com/aws/aws-sdk-net/issues/3610
            RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED
        });

        _transferUtility = new TransferUtility(_client);
    }
    
    public void TransferFile(string name, byte[] data, string? contentType = null)
    {
        using MemoryStream memoryStream = new MemoryStream(data);
        TransferFile(name, memoryStream, contentType);
    }

    public void TransferFile(string name, Stream inputStream, string? contentType = null)
    {
        TransferUtilityUploadRequest request = new TransferUtilityUploadRequest()
        {
            BucketName = _settings.Bucket,
            Key = name,
            InputStream = inputStream,
            CannedACL = S3CannedACL.Private,
            AutoResetStreamPosition = true,
            AutoCloseStream = false
        };

        if (contentType != null)
        {
            request.ContentType = contentType;
        }

        _transferUtility.Upload(request);
    }

    public Task<string> GetPreSignedUrlForFile(string name, HttpVerb verb, int validityMinutes = 60)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
        {
            BucketName = _settings.Bucket,
            Key = name,
            Expires = DateTime.UtcNow.AddMinutes(validityMinutes),
            Verb = verb
        };

        return _client.GetPreSignedURLAsync(request);
    }

    public async Task<byte[]> DownloadFile(string name)
    {
        GetObjectResponse response = await _client.GetObjectAsync(new GetObjectRequest()
        {
            BucketName = _settings.Bucket,
            Key = name
        });

        using MemoryStream memoryStream = new MemoryStream();
        
        await response.ResponseStream.CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    public async Task DeleteFile(string name)
    {
        DeleteObjectRequest request = new DeleteObjectRequest()
        {
            BucketName = _settings.Bucket,
            Key = name
        };

        await _client.DeleteObjectAsync(request);
    }
}
