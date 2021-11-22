using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace CursoEFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Método a ser executado
        }

        private static void InserirDados()
        {
            using var db = new Data.ApplicationContext();

            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            db.Produtos.Add(produto);
            var registros = db.SaveChanges();
            Console.WriteLine($"Total registro(s): {registros}");
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Gabriel",
                CEP = "00000000",
                Cidade = "Jaú",
                Estado = "SP",
                Telefone = "9987654321",
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Teste 1",
                    CEP = "11111111",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Telefone = "9987654322",
                },

                new Cliente
                {
                    Nome = "Teste 2",
                    CEP = "22222222",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Telefone = "9987654323",
                },
        };

            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);
            db.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total registro(s): {registros}");
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            var consulta = db.Clientes
                .AsNoTracking()
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();

            foreach(var cliente in consulta)
            {
                Console.WriteLine($"Consultando cliente: {cliente.Id}");
                db.Clientes.Find(cliente.Id);
                //db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                Observacao = "Pedido teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            //var cliente = db.Clientes.FirstOrDefault(p => p.Id == 2);
            var cliente = db.Clientes.Find(2);
            
            cliente.Nome = "Cliente nome alterado";

            // db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(3);
            db.Remove(cliente);
            db.SaveChanges();
        }
    }
}