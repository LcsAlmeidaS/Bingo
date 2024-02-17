using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
public class Cartela
{
    private int[,] matriz;

    public Cartela()
    {
        matriz = new int[5, 5];
    }

    public void InserirValor(int linha, int coluna, int valor)
    {
        if (linha < 0 || linha > 4) // Verifica se o valor da linha está fora dos limites válidos para a matriz. Se estiver fora o return é aplicado, interrompendo a execução do método
            return;
        if (coluna < 0 || coluna > 4)
            return;
        matriz[linha, coluna] = valor; // Se os ifs forem falsos, significa que L e C estão dentro dos limites permitidos, então VALOR é inserido na posição correspondente da matriz
    }

    public int GetValorDaMatriz(int linha, int coluna) // Método para obter o valor armazenado em uma posição específica
    {
        if (linha < 0 || linha > 4) // Se estiver fora do limite retorna -1, que significa um indice inválido
            return -1;
        if (coluna < 0 || coluna > 4)
            return -1;
        return matriz[linha, coluna]; // Se tudo estiver ok, retorna o valor armazenado na matriz da posição especificada pelos índices 
    }
}
public class Jogador
{
    private string nome;
    private string idade;
    private string sexo;
    private bool marcouBingo;
    private Cartela[] cartelasDoBingo;
    private Cartela[] cartelasPreenchidas;

    public Jogador(Cartela[] cartelasDoBingo, string nome, string idade, string sexo)
    {
        this.nome = nome;
        this.idade = idade;
        this.sexo = sexo;
        this.cartelasDoBingo = cartelasDoBingo;
        cartelasPreenchidas = new Cartela[this.cartelasDoBingo.Length]; // Cria um novo array de Cartela com o mesmo comprimento que o array de cartelas do bingo e inicializa cada elemento do array com uma nova instância de Cartela.

        for (int indice = 0; indice < this.cartelasPreenchidas.Length; indice++)
        {
            cartelasPreenchidas[indice] = new Cartela();
        }
    }

    public string GetNome()
    {
        return nome;
    }

    public string GetIdade()
    {
        return idade;
    }

    public string GetSexo()
    {
        return sexo;
    }

    public bool GetMarcouBingo()
    {
        return marcouBingo;
    }


    public void DefinirSeMarcouBingo(bool marcouBingo) // Atribui um valor bool caso ele tenha marcado bingo ou não. Ex: name="marcouBingo">true: marcou bingo | false: não marcou bingo. 
    {
        this.marcouBingo = marcouBingo;
    }

    public Cartela[] GetNumeracaoDasCartelasSorteadas()
    {
        return cartelasDoBingo;
    }

    public Cartela[] GetMarcacaoNasCartelas()
    {
        return cartelasPreenchidas;
    }

    public void MarcarNaCartela(int numero) // Marca a cartela caso encontre o valor sorteado em suas cartelas.
    {
        int indiceCartelaMarcada = 0;
        for (int indiceCartelaDoBingo = 0; indiceCartelaDoBingo < cartelasDoBingo.Length; indiceCartelaDoBingo++)
        {
            for (int linha = 0; linha < 5; linha++)
            {
                for (int coluna = 0; coluna < 5; coluna++)
                {
                    int valor = cartelasDoBingo[indiceCartelaDoBingo].GetValorDaMatriz(linha, coluna); // Obtém o valor na posição atual da cartela do bingo
                    if (linha == 2 && coluna == 2) continue; // Se a posição atual for 2 2, o método ignora e passa para a próxima posição
                    if (numero == valor) // Refere a cartela atual que está sendo marcada. Marcando a posição especificada na cartela atual com o valor fornecido
                        cartelasPreenchidas[indiceCartelaMarcada].InserirValor(linha, coluna, valor);
                }
            }
            indiceCartelaMarcada++;
        }
    }

    public bool GritarBingo() // Grita bingo, ou seja, confere linhas, colunas e diagonais e verifica se alguma delas está preenchida.
    {
        for (int indice = 0; indice < cartelasDoBingo.Length; indice++) // Percorre todas as cartealas do jogador
        {
            bool linhasValidas = VerificarLinhas(cartelasDoBingo[indice], cartelasPreenchidas[indice]); // Recebe duas cartelas como parâmetros: a cartela sorteada e a cartela preenchida.
            bool colunasValidas = VerificarColunas(cartelasDoBingo[indice], cartelasPreenchidas[indice]);
            bool diagonalPrincipalValida = VerificarDiagonalPrincipal(cartelasDoBingo[indice], cartelasPreenchidas[indice]);
            bool diagonalSecundariaValida = VerificarDiagonalSecundaria(cartelasDoBingo[indice], cartelasPreenchidas[indice]);

            bool venceu = linhasValidas || colunasValidas || diagonalPrincipalValida || diagonalSecundariaValida;
            if (!venceu) continue; // Se não venceu, continua
            else return true;
        }
        return false;
    }

    private bool VerificarLinhas(Cartela cartelaSorteada, Cartela cartelaMarcada) // Verifica se alguma linha está preenchida na Cartela. cartelaSorteada = Cartela com valor random, cartelaMarcada = Cartela que será marcada com os valores.
    {
        int indice = 0;
        bool[] linhasValidasOuNao = new bool[5]; // Essa variável será usada para rastrear qual linha está sendo verificada e o array será usado para armazenar o estado de cada linha.
        bool encontrouValorDiferente = false;
        for (int linha = 0; linha < 5; linha++)
        {
            for (int coluna = 0; coluna < 5; coluna++)
            {
                int valorDaCartelaSorteada = cartelaSorteada.GetValorDaMatriz(linha, coluna); // Obtém o valor na posição atual da cartela sorteada e da cartela preenchida.
                int valorDaCartelaMarcada = cartelaMarcada.GetValorDaMatriz(linha, coluna);
                if (!encontrouValorDiferente) // Se o valor na posição atual da cartela sorteada for diferente do valor na posição atual da cartela preenchida, o método define a variável encontrouValorDiferente como true.
                    if (valorDaCartelaSorteada != valorDaCartelaMarcada)
                        encontrouValorDiferente = true;

            }
            // Se encontrouValorDiferente for true, isso significa que a linha não está preenchida, então o método define o elemento correspondente no array linhasValidasOuNao como false.Se encontrouValorDiferente for false, isso significa que a linha está preenchida, então o método define o elemento correspondente no array linhasValidasOuNao como true.
            if (encontrouValorDiferente)
                linhasValidasOuNao[indice] = false;
            else
                linhasValidasOuNao[indice] = true;
            indice++;
        }
        // Depois de verificar todas as linhas, o método entra em outro loop que verifica se alguma linha está preenchida. Se alguma linha estiver preenchida, o método retorna true.
        for (int i = 0; i < 5; i++)
            if (linhasValidasOuNao[i]) return true;
        return false;
    }

    private bool VerificarColunas(Cartela cartelaSorteada, Cartela cartelaMarcada) // Verifica se alguma coluna está preenchida na Cartela. cartelaSorteada = Cartela com valor random, cartelaMarcada = Cartela que será marcada com os valores.
    {
        int indice = 0;
        bool[] colunasValidasOuNao = new bool[5];
        bool encontrouValorDiferente = false; ;
        for (int coluna = 0; coluna < 5; coluna++)
        {
            for (int linha = 0; linha < 5; linha++)
            {
                int valorDaCartelaSorteada = cartelaSorteada.GetValorDaMatriz(linha, coluna);
                int valorDaCartelaMarcada = cartelaMarcada.GetValorDaMatriz(linha, coluna);
                if (!encontrouValorDiferente)
                    if (valorDaCartelaSorteada != valorDaCartelaMarcada)
                        encontrouValorDiferente = true;
            }
            if (encontrouValorDiferente)
                colunasValidasOuNao[indice++] = false;
            else
                colunasValidasOuNao[indice++] = true;
        }
        for (int i = 0; i < 5; i++)
            if (colunasValidasOuNao[i]) return true;
        return false;
    }

    private bool VerificarDiagonalPrincipal(Cartela cartelaSorteada, Cartela cartelaMarcada) // Verifica se a diagonal principal está preenchida. cartelaSorteada = Cartela com valor random, cartelaMarcada = Cartela que será marcada com os valores.
    {
        for (int linha = 0; linha < 5; linha++)
        {
            for (int coluna = 0; coluna < 5; coluna++)
            {
                int valorDaCartelaSorteada = cartelaSorteada.GetValorDaMatriz(linha, coluna);
                int valorDaCartelaMarcada = cartelaMarcada.GetValorDaMatriz(linha, coluna);
                if (linha == coluna && valorDaCartelaMarcada != valorDaCartelaSorteada) return false;
            }
        }
        return true;
    }

    private bool VerificarDiagonalSecundaria(Cartela cartelaSorteada, Cartela cartelaMarcada) // Verifica se a diagonal secundária está preenchida .cartelaSorteada = Cartela com valor random, cartelaMarcada = Cartela que será marcada com os valores.
    {
        int linha = 0;
        for (int coluna = 5; coluna >= 0; coluna--)
        {
            int valorDaCartelaSorteada = cartelaSorteada.GetValorDaMatriz(linha, coluna);
            int valorDaCartelaMarcada = cartelaMarcada.GetValorDaMatriz(linha, coluna);
            if (valorDaCartelaMarcada != valorDaCartelaSorteada) return false;
            linha++;
        }
        return true;
    }
}
public class Jogo
{
    private int quantidadeSorteada;
    private int quantidadeDeJogadoresAtual;
    private int posicaoDoJogadorNaClassificacao;
    private int[] numerosSorteados;
    private int[] quantidadeDeCartelas;
    private string[] nomesDosJogadoresDaPartida;
    private string[] idadeDosJogadores;
    private string[] sexoDosJogadores;
    private Jogador[] jogadoresDaPartida;
    private Jogador[] classificacao;
    private Random random;

    public Jogo(int quantidadeDeJogadores, int[] quantidadeDeCartelasPorJogadores, string[] nomesDosJogadores, string[] idadeDosJogadores, string[] sexoDosJogadores)
    {
        quantidadeSorteada = 0;
        quantidadeDeJogadoresAtual = quantidadeDeJogadores;
        posicaoDoJogadorNaClassificacao = 0;
        numerosSorteados = new int[75];
        quantidadeDeCartelas = quantidadeDeCartelasPorJogadores;
        nomesDosJogadoresDaPartida = nomesDosJogadores;
        this.idadeDosJogadores = idadeDosJogadores;
        this.sexoDosJogadores = sexoDosJogadores;
        jogadoresDaPartida = new Jogador[quantidadeDeJogadores];
        classificacao = new Jogador[quantidadeDeJogadores];
        random = new Random();
    }

    public int GetQuantidadeSorteada()
    {
        return quantidadeSorteada;
    }

    public int GetQuantidadeDeJogadoresAtual()
    {
        return quantidadeDeJogadoresAtual;
    }

    public int GetPosicaoDoJogadorNaClassificacao()
    {
        return posicaoDoJogadorNaClassificacao;
    }

    public int[] GetNumerosSorteados()
    {
        return numerosSorteados;
    }

    public int[] GetQuantidadeDeCartelas()
    {
        return quantidadeDeCartelas;
    }

    public string[] GetNomesDosJogadoresDaPartida()
    {
        return nomesDosJogadoresDaPartida;
    }

    public Jogador[] GetJogadoresDaPartida()
    {
        return jogadoresDaPartida;
    }

    public Jogador[] GetClassificacao()
    {
        return classificacao;
    }

    public void Iniciar() // Responsável por iniciar o Loop do Jogo até que sobre somente um jogador na partida ou todos os 75 números forem sorteados.
    {
        AtribuirCartelasAosJogadores();

        while (true)
        {
            if (quantidadeDeJogadoresAtual == 1) // Loop terminará até que sobre um único jogador
            {
                classificacao[posicaoDoJogadorNaClassificacao] = jogadoresDaPartida[0]; // Se for verdadeiro o jogador é adicionado ao ranking
                break;
            }
            if (quantidadeSorteada == 75)
            {
                Console.WriteLine("TODOS OS NÚMEROS FORAM SORTEADOS");
                Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR?");
                Console.ReadKey();
                break;
            }
            int valorSorteado = SortearNumero();

            Console.Clear();
            Console.WriteLine($"O NÚMERO SORTEADO FOI {valorSorteado}");
            Console.WriteLine($"QUANTIDADE SORTEADA ATÉ AGORA {quantidadeSorteada}/75");
            Console.WriteLine();
            MarcarValorNaCartelaDeCadaJogador(valorSorteado); // Este método marca o número sorteado em todas as cartelas dos jogadores

            int opcao = SelecionarOpcao();

            while (opcao == 3)
            {
                Console.WriteLine();
                for (int i = 0; i < jogadoresDaPartida.Length; i++)
                {
                    if (jogadoresDaPartida[i] == null) continue;
                    Console.WriteLine($"Cartelas do Jogador: " +
                        $"Nome: {jogadoresDaPartida[i].GetNome()} | " +
                        $"Idade: {jogadoresDaPartida[i].GetIdade()} | " +
                        $"Sexo: {jogadoresDaPartida[i].GetSexo()}");
                    Console.WriteLine();
                    MostrarMarcacaoNaCartelaDeUmJogador(jogadoresDaPartida[i]);
                }
                Console.WriteLine();
                Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                Console.ReadKey();

                Console.Clear();
                Console.WriteLine("O QUE VOCÊ DESEJA FAZER?");
                Console.WriteLine("[1] Gritar Bingo\n[2] Pular para o próximo sorteio\n[3] Ver marcação nas cartelas\n[4] Ver cartelas sorteadas");
                Console.Write("DIGITE A OPÇÃO (EM NÚMEROS): ");

                opcao = int.Parse(Console.ReadLine());
            }

            while (opcao == 4)
            {
                Console.WriteLine();
                for (int i = 0; i < jogadoresDaPartida.Length; i++)
                {
                    if (jogadoresDaPartida[i] == null) continue;
                    Console.WriteLine($"Cartelas do Jogador: " +
                        $"Nome: {jogadoresDaPartida[i].GetNome()} | " +
                        $"Idade: {jogadoresDaPartida[i].GetIdade()} | " +
                        $"Sexo: {jogadoresDaPartida[i].GetSexo()}");
                    Console.WriteLine();
                    MostrarNumerosSorteadosParaUmJogador(jogadoresDaPartida[i]);
                }
                Console.WriteLine();
                Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                Console.ReadKey();

                Console.Clear();
                Console.WriteLine("O QUE VOCÊ DESEJA FAZER?");
                Console.WriteLine("[1] Gritar Bingo\n[2] Pular para o próximo sorteio\n[3] Ver marcação nas cartelas\n[4] Ver cartelas sorteadas");
                Console.Write("DIGITE A OPÇÃO (EM NÚMEROS): ");

                opcao = int.Parse(Console.ReadLine());
            }

            switch (opcao)
            {
                case 1:
                    bool bingo = jogadoresDaPartida[0].GritarBingo();
                    JogadorGritouBingo(bingo);
                    break;
                case 2:
                    Console.Clear();
                    break;
                case 3:
                    Console.Clear();
                    for (int i = 0; i < jogadoresDaPartida.Length; i++)
                    {
                        if (jogadoresDaPartida[i] == null) continue;
                        Console.WriteLine($"Cartelas do Jogador: {jogadoresDaPartida[i].GetNome()}");
                        Console.WriteLine();
                        MostrarMarcacaoNaCartelaDeUmJogador(jogadoresDaPartida[i]);
                    }
                    Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                    Console.ReadKey();
                    break;
                case 4:
                    Console.Clear();
                    for (int i = 0; i < jogadoresDaPartida.Length; i++)
                    {
                        if (jogadoresDaPartida[i] == null) continue;
                        Console.WriteLine($"Cartelas do Jogador: {jogadoresDaPartida[i].GetNome()}");
                        Console.WriteLine();
                        MostrarNumerosSorteadosParaUmJogador(jogadoresDaPartida[i]);
                    }
                    Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                    Console.ReadKey();
                    break;
                default:
                    throw new Exception("OPÇÃO INVÁLIDA, ENCERRANDO A APLICAÇÃO.");
            }
        }
        Console.Clear();
        MostrarRankingDaPartida();
    }

    private void AtribuirCartelasAosJogadores()
    {
        for (int i = 0; i < jogadoresDaPartida.Length; i++)
        {
            Cartela[] cartelas = new Cartela[quantidadeDeCartelas[i]];
            for (int j = 0; j < cartelas.Length; j++)
            {
                cartelas[j] = new Cartela();
            }
            DistribuirValoresAsCartelas(cartelas);
            jogadoresDaPartida[i] = new Jogador(cartelas, nomesDosJogadoresDaPartida[i], idadeDosJogadores[i], sexoDosJogadores[i]);
        }
    }

    private void DistribuirValoresAsCartelas(Cartela[] cartelas)
    {
        foreach (Cartela cartela in cartelas)
        {
            int valorMinimo = 1;
            int valorMaximo = 15;
            int indiceArrayDeValores = 0;
            int[] valores = new int[valorMaximo];
            for (int coluna = 0; coluna < 5; coluna++)
            {
                for (int linha = 0; linha < 5; linha++)
                {
                    if (coluna == 2 && linha == 2) continue;
                    int valor = random.Next(valorMinimo, valorMaximo + 1);
                    bool valorExiste = false;
                    foreach (int v in valores)
                    {
                        if (v == valor)
                        {
                            valorExiste = true;
                            break;
                        }
                    }
                    while (valorExiste)
                    {
                        valor = random.Next(valorMinimo, valorMaximo + 1);
                        valorExiste = false;
                        foreach (int v in valores)
                        {
                            if (v == valor)
                            {
                                valorExiste = true;
                                break;
                            }
                        }
                    }
                    cartela.InserirValor(linha, coluna, valor);
                    valores[indiceArrayDeValores++] = valor;
                }
                valorMinimo += 15;
                valorMaximo += 15;
                indiceArrayDeValores = 0;
            }
        }
    }

    private void JogadorGritouBingo(bool bingo) // Quando o Jogador gritar bingo este método define se o colocará na primeira ou na última posição do Ranking.
    {
        if (bingo)
        {
            jogadoresDaPartida[0].DefinirSeMarcouBingo(true);
            classificacao[posicaoDoJogadorNaClassificacao++] = jogadoresDaPartida[0];
            quantidadeDeJogadoresAtual--;

            Jogador[] atualizarJogadores = new Jogador[quantidadeDeJogadoresAtual];
            for (int i = 1; i < quantidadeDeJogadoresAtual; i++)
            {
                atualizarJogadores[i - 1] = jogadoresDaPartida[i];
            }
            jogadoresDaPartida = atualizarJogadores;
            Console.Clear();
            Console.WriteLine("JOGADOR FOI ADICIONADO NO RANKING!");
            Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
            Console.ReadKey();
        }
        else
        {
            jogadoresDaPartida[0].DefinirSeMarcouBingo(false);
            quantidadeDeJogadoresAtual--;
            Jogador[] atualizarJogadores = new Jogador[quantidadeDeJogadoresAtual];
            for (int i = 1; i <= quantidadeDeJogadoresAtual; i++)
            {
                atualizarJogadores[i - 1] = jogadoresDaPartida[i];
            }
            classificacao[classificacao.Length - 1] = jogadoresDaPartida[0];
            jogadoresDaPartida = atualizarJogadores;
            Console.Clear();
            Console.WriteLine("BINGO INVÁLIDO!");
            Console.WriteLine("JOGADOR FOI REMOVIDO DA PARTIDA!");
            Console.WriteLine("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
            Console.ReadKey();
        }
    }

    private int SortearNumero() // Sorteia um número aleatório que ainda não foi sorteado.
    {
        int valorSorteado = random.Next(1, 76);
        bool valorExiste = false;
        foreach (int v in numerosSorteados)
        {
            if (v == valorSorteado)
            {
                valorExiste = true;
                break;
            }
        }
        while (valorExiste)
        {
            valorSorteado = random.Next(1, 76);
            valorExiste = false;
            foreach (int v in numerosSorteados)
            {
                if (v == valorSorteado)
                {
                    valorExiste = true;
                    break;
                }
            }
        }
        numerosSorteados[quantidadeSorteada++] = valorSorteado;
        return valorSorteado;
    }


    private int SelecionarOpcao()
    {
        Console.WriteLine("O QUE VOCÊ DESEJA FAZER?");
        Console.WriteLine("[1] Gritar Bingo\n[2] Sortear outro número\n[3] Ver cartelas\n[4] Ver cartelas sorteadas");
        Console.Write("DIGITE A OPÇÃO (EM NÚMEROS): ");

        int opcao = int.Parse(Console.ReadLine());
        return opcao;
    }

    private void MarcarValorNaCartelaDeCadaJogador(int valor)
    {
        foreach (Jogador jogador in jogadoresDaPartida)
        {
            if (jogador == null) continue;
            jogador.MarcarNaCartela(valor);
        }
    }

    private void MostrarMarcacaoNaCartelaDeUmJogador(Jogador jogador)
    {
        Cartela[] cartelas = jogador.GetMarcacaoNasCartelas();
        for (int indice = 0; indice < cartelas.Length; indice++)
        {
            for (int linha = 0; linha < 5; linha++)
            {
                for (int coluna = 0; coluna < 5; coluna++)
                {
                    string valorNaCartela = cartelas[indice].GetValorDaMatriz(linha, coluna).ToString();
                    if (valorNaCartela.Length == 1)
                    {
                        valorNaCartela = "0" + valorNaCartela;
                    }
                    Console.Write(valorNaCartela + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    private void MostrarNumerosSorteadosParaUmJogador(Jogador jogador)
    {
        Cartela[] cartelas = jogador.GetNumeracaoDasCartelasSorteadas();
        for (int indice = 0; indice < cartelas.Length; indice++)
        {
            for (int linha = 0; linha < 5; linha++)
            {
                for (int coluna = 0; coluna < 5; coluna++)
                {
                    string valorNaCartela = cartelas[indice].GetValorDaMatriz(linha, coluna).ToString();
                    if (valorNaCartela.Length == 1)
                    {
                        valorNaCartela = "0" + valorNaCartela;
                    }
                    Console.Write(valorNaCartela + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    private void MostrarRankingDaPartida()
    {
        int i = 0;
        foreach (Jogador jogador in classificacao)
        {
            if (jogador == null) continue;
            string marcouBingo = jogador.GetMarcouBingo() ? "MARCOU BINGO :D" : "NÃO MARCOU BINGO :(";
            Console.WriteLine($"{i + 1}º LUGAR: Nome: {jogador.GetNome()} | Idade: {jogador.GetIdade()} | Sexo: {jogador.GetSexo()} => {marcouBingo}");
            i++;
        }
    }
}
public class Program
{
    public static void Main()
    {
        // Define quantidade de Jogadores
        Console.Write("BINGO DO LUQUINHA:\nINFORME A QUANTIDADE DE JOGADORES DA PARTIDA: ");
        int quantidadeDeJogadores = int.Parse(Console.ReadLine());

        int[] quantidadeDeCartelasPorJogador = new int[quantidadeDeJogadores];
        for (int i = 0; i < quantidadeDeCartelasPorJogador.Length; i++)
        {
            Console.Write($"INFORME A QUANTIDADE DE CARTELAS DO {i + 1}º JOGADOR: ");
            int quantidadeDeCartelas = int.Parse(Console.ReadLine());

            // Define quantidade de cartelas por Jogador
            while (!(quantidadeDeCartelas >= 1 && quantidadeDeCartelas <= 4))
            {
                Console.Clear();
                Console.WriteLine($"UM JOGADOR PRECISA TER ENTRE 1 A 4 CARTELAS");
                Console.Write($"INFORME A QUANTIDADE DE CARTELAS DO {i + 1}º JOGADOR: ");
                quantidadeDeCartelas = int.Parse(Console.ReadLine());
            }

            quantidadeDeCartelasPorJogador[i] = quantidadeDeCartelas;
        }

        Console.WriteLine("INFORME O NOME DOS JOGADORES:");

        // Define os nomes dos Jogadores
        string[] nomesDosJogadores = new string[quantidadeDeJogadores];
        for (int i = 0; i < quantidadeDeJogadores; i++)
        {
            Console.Write($"Nome do {i + 1}º Jogador: ");
            string nome = Console.ReadLine();

            while (nome == null || nome == "")
            {
                Console.Clear();
                Console.WriteLine("INSIRA UM NOME VÁLIDO PARA O JOGADOR");
                Console.Write("Nome do primeiro Jogador: ");
                nome = Console.ReadLine();
            }
            while (nome == null || nome == "")
            {
                Console.Clear();
                Console.WriteLine("INSIRA UM NOME VÁLIDO PARA O JOGADOR");
                Console.Write("Nome do primeiro Jogador: ");
                nome = Console.ReadLine();
            }
            nomesDosJogadores[i] = nome;
        }

        Console.WriteLine("INFORME A IDADE DOS JOGADORES:");

        // Define os nomes dos Jogadores
        string[] idadeDosJogadores = new string[quantidadeDeJogadores];
        for (int i = 0; i < quantidadeDeJogadores; i++)
        {
            Console.Write($"Idade do {i + 1}º Jogador: ");
            string idade = Console.ReadLine();

            while (idade == null || idade == "")
            {
                Console.Clear();
                Console.WriteLine("INSIRA UMA IDADE VÁLIDA PARA O JOGADOR");
                Console.Write("Idade do Jogador: ");
                idade = Console.ReadLine();
            }
            while (idade == null || idade == "")
            {
                Console.Clear();
                Console.WriteLine("INSIRA UMA IDADE VÁLIDA PARA O JOGADOR");
                Console.Write("Idade do Jogador: ");
                idade = Console.ReadLine();
            }
            idadeDosJogadores[i] = idade;
        }

        Console.WriteLine("INFORME O SEXO DOS JOGADORES:");

        // Define os nomes dos Jogadores
        string[] sexoDosJogadores = new string[quantidadeDeJogadores];
        for (int i = 0; i < quantidadeDeJogadores; i++)
        {
            Console.Write($"Sexo do {i + 1}º Jogador: ");
            string sexo = Console.ReadLine();

            while (sexo == null || sexo == "")
            {
                Console.Clear();
                Console.WriteLine("INSIRA UMA SEXO VÁLIDO PARA O JOGADOR");
                Console.Write("Sexo do Jogador: ");
                sexo = Console.ReadLine();
            }
            while (sexo == null || sexo == "")
            {
                Console.Clear();
                Console.WriteLine("INSIRA UM SEXO VÁLIDO PARA O JOGADOR");
                Console.Write("Sexo do Jogador: ");
                sexo = Console.ReadLine();
            }
            while (!(sexo.Equals("M") || sexo.Equals("F")))
            {
                Console.Clear();
                Console.WriteLine("INSIRA UM SEXO VÁLIDO PARA O JOGADOR");
                Console.Write("Sexo do Jogador: ");
                sexo = Console.ReadLine();
            }
            sexoDosJogadores[i] = sexo;
        }


        // Inicia o Jogo
        Jogo jogo = new Jogo(quantidadeDeJogadores, quantidadeDeCartelasPorJogador, nomesDosJogadores, idadeDosJogadores, sexoDosJogadores);
        jogo.Iniciar();
        Console.WriteLine("PRESSIONA ALGUMA TECLA PARA PROSSEGUIR...");
        Console.ReadKey();
    }
}