using Azure.Storage.Blobs;

namespace WebAPI.Utils.BlobStorage
{
    public static class AzureBlobStorageHelper
    {
        public static async Task<string> UploadImageBlobAsync(IFormFile arquivo, string stringConexao, string nomeContainer)
        {
			try
			{
				//verifica se tem um arquivo
				if (arquivo != null)
				{
					
					//gera nome unico + extensao do arquivo
					var blobName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(arquivo.FileName);

					//cria uma instancia do client Blob Service e passa a string de conexao
					var blobServiceClient = new BlobServiceClient(stringConexao);

					//obtem um container client usando o nome do container do blob
					var blobContainerClient = blobServiceClient.GetBlobContainerClient(nomeContainer);

					//obtem um blob client usando o blob name
					var blobClient = blobContainerClient.GetBlobClient(blobName);

					//abre o fluxo de entrada do arquivo(foto)
					using(var stream = arquivo.OpenReadStream())
					{
						//carrega o arquivo(foto) para o blob storage de forma assincrona
						await blobClient.UploadAsync(stream, true);
					}

					//retorna a uri do blob como uma string
					return blobClient.Uri.ToString();


				}
				else
				{
					//retorna a uri de uma imagem padrao caso nenhum arquivo seja enviado
					return "https://blobvitalhubfernando.blob.core.windows.net/containervitalhubfernando/profilepattern.png";
				}
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
