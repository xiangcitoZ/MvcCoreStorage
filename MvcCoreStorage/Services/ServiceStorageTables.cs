using Azure.Data.Tables;
using MvcCoreStorage.Models;

namespace MvcCoreStorage.Services
{
    public class ServiceStorageTables
    {
        private TableClient tableCliente;

        public ServiceStorageTables(TableServiceClient tableService)
        {
            this.tableCliente = tableService.GetTableClient("clientes");

            Task.Run(async () =>
            {
                await this.tableCliente.CreateIfNotExistsAsync();
            });
        }

        public async Task
            CreateClientAsync(int id, string nombre, int salario
            , int edad, string empresa)
        {
            Cliente cliente = new Cliente();
            cliente.IdCliente = id;
            cliente.Nombre = nombre;
            cliente.Salario = salario;
            cliente.Edad = edad;
            cliente.Empresa = empresa;
            await this.tableCliente.AddEntityAsync<Cliente>(cliente);
        }

        public async Task<Cliente> FindClienteAsync
            (string partitionKey, string rowkey)
        {
            Cliente cliente = await 
                this.tableCliente.GetEntityAsync<Cliente>
                (partitionKey, rowkey);
            return cliente;
        }

        public async Task DeleteClienteAsync
            (string partitionKey, string rowkey)
        {
            await this.tableCliente.DeleteEntityAsync
                (partitionKey, rowkey);
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            List<Cliente> clientes = new List<Cliente>();
            var query = 
                this.tableCliente.QueryAsync<Cliente>
                (filter: "");
            await foreach(Cliente item in query)
            {
                clientes.Add(item);
            }
            return clientes;
        }

    }
}
