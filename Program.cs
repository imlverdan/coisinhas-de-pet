using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class Transacao
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; }
}

public class ControleFinanceiro
{
    private static List<Transacao> registros = new List<Transacao>();

    public static void Main(string[] args)
    {
        Console.Title = "Controle Financeiro PetShop - Jullyana";
        bool executando = true;

        while (executando)
        {
            ExibirMenu();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    RegistrarTransacao("Receita");
                    break;
                case "2":
                    RegistrarTransacao("Despesa");
                    break;
                case "3":
                    GerarRelatorio();
                    break;
                case "4":
                    executando = false;
                    Console.WriteLine("\nObrigado por usar a ferramenta. Os dados não foram salvos.");
                    break;
                default:
                    Console.WriteLine("\nOpção inválida. Tente novamente.");
                    break;
            }
            if (executando)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private static void ExibirMenu()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("    MENU - CONTROLE FINANCEIRO PET");
        Console.WriteLine("=======================================");
        Console.WriteLine("1. Registrar Receita (Venda)");
        Console.WriteLine("2. Registrar Despesa (Custo, Material)");
        Console.WriteLine("3. Gerar Relatório (Fluxo de Caixa)");
        Console.WriteLine("4. Sair");
        Console.Write("\nEscolha uma opção: ");
    }

    private static void RegistrarTransacao(string tipo)
    {
        Console.Clear();
        Console.WriteLine($"\n--- REGISTRAR {tipo.ToUpper()} ---");

        Console.Write("Descrição (Ex: Venda de Colete M ou Compra de Tecido): ");
        string descricao = Console.ReadLine();

        decimal valor = 0;
        bool valorValido = false;

        var culturaBrasileira = new CultureInfo("pt-BR");

        while (!valorValido)
        {
            Console.Write($"Valor da {tipo} (Use vírgula, ex: 45,50): R$ ");
            string entrada = Console.ReadLine();

            // CORREÇÃO: Usando a cultura "pt-BR" para TryParse
            if (decimal.TryParse(entrada, NumberStyles.Currency, culturaBrasileira, out valor) && valor > 0)
            {
                valorValido = true;
            }
            else
            {
                Console.WriteLine("Valor inválido. Por favor, insira um número positivo usando a vírgula (Ex: 45,50).");
            }
        }

        registros.Add(new Transacao
        {
            Descricao = descricao,
            Valor = valor,
            Tipo = tipo
        });

        Console.WriteLine($"\nRegistro de {tipo} realizado com sucesso!");
    }

    private static void GerarRelatorio()
    {
        Console.Clear();
        Console.WriteLine("=======================================");
        Console.WriteLine("        RELATÓRIO FINANCEIRO");
        Console.WriteLine("=======================================");

        if (registros.Count == 0)
        {
            Console.WriteLine("Nenhum registro encontrado.");
            return;
        }

        decimal totalReceitas = registros
            .Where(r => r.Tipo == "Receita")
            .Sum(r => r.Valor);

        decimal totalDespesas = registros
            .Where(r => r.Tipo == "Despesa")
            .Sum(r => r.Valor);

        decimal saldo = totalReceitas - totalDespesas;

        var culturaBrasileira = new CultureInfo("pt-BR");

        Console.WriteLine("\n--- Histórico de Transações ---");
        foreach (var r in registros)
        {
            string sinal = r.Tipo == "Receita" ? "+" : "-";
            string valorFormatado = String.Format(culturaBrasileira, "{0:N2}", r.Valor);
            Console.WriteLine($"[{r.Tipo,-8}] {r.Descricao,-40} R$ {sinal}{valorFormatado}");
        }

        Console.WriteLine("\n---------------------------------------");
        Console.WriteLine($"TOTAL DE RECEITAS (Entradas): R$ {String.Format(culturaBrasileira, "{0:N2}", totalReceitas)}");
        Console.WriteLine($"TOTAL DE DESPESAS (Saídas): R$ {String.Format(culturaBrasileira, "{0:N2}", totalDespesas)}");
        Console.WriteLine("---------------------------------------");

        Console.ForegroundColor = saldo >= 0 ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"SALDO (LUCRO) ATUAL: R$ {String.Format(culturaBrasileira, "{0:N2}", saldo)}");
        Console.ResetColor();

        Console.WriteLine("---------------------------------------");
    }
}
