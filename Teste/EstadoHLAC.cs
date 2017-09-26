using System;
using System.Collections.Generic;
using System.Text;
using Busca;
using Busca.Grafo;
using Busca.Util;

namespace Teste
{
    class EstadoHLAC : Estado
    {
        public const int MARGEM_INICIAL = 0;
        public const int MARGEM_FINAL = 1;

        private readonly int homem, lobo, carneiro, alface, h;
        private readonly string operacao;

        public EstadoHLAC(int homem, int lobo, int carneiro, int alface, string operacao)
        {
            this.homem = homem;
            this.lobo = lobo;
            this.carneiro = carneiro;
            this.alface = alface;
            this.operacao = operacao;
            h = CalcularH();
        }

        public override string ToString()
        {
            // para poder imprimir a solução completa na tela, incluindo também
            // quem estava de qual lado
            string margemInicial = "";
            string margemFinal = "";
            if (homem == MARGEM_INICIAL)
            {
                margemInicial += "H";
            }
            else
            {
                margemFinal += "H";
            }
            if (lobo == MARGEM_INICIAL)
            {
                margemInicial += "L";
            }
            else
            {
                margemFinal += "L";
            }
            if (carneiro == MARGEM_INICIAL)
            {
                margemInicial += "C";
            }
            else
            {
                margemFinal += "C";
            }
            if (alface == MARGEM_INICIAL)
            {
                margemInicial += "A";
            }
            else
            {
                margemFinal += "A";
            }
            return margemInicial + "|" + margemFinal + " (" + operacao + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            EstadoHLAC e = (obj as EstadoHLAC);
            return (e != null &&
                e.homem == homem &&
                e.lobo == lobo &&
                e.carneiro == carneiro &&
                e.alface == alface);
        }

        public override int GetHashCode()
        {
            // quando o hash code for "simples", ele pode ser calculado diretamente,
            // sem ser armazenado em uma variável
            return (homem * 11) + (lobo * 7) + (carneiro * 5) + (alface * 3);
        }

        private int CalcularH()
        {
            return 4 - (lobo + carneiro + alface + homem);
        }

        public int Custo
        {
            get
            {
                // todas as nossas operações têm o mesmo custo: 1
                // (não existe operação mais complexa que outra, ou mais
                // demorada que outra)
                return 1;
            }
        }

        public string Descricao
        {
            get
            {
                return
                    "Uma pessoa, um lobo, um carneiro e um cesto de alface estao a beira " +
                    "de um rio. Dispondo de um barco no qual pode carregar apenas a pessoa e " +
                    "mais um dos outros tres, a pessoa deve transportar tudo para a outra margem. " +
                    "Determine uma serie de travessias que respeite a seguinte condicao: " +
                    "em nenhum momento devem ser deixados juntos, sem a supervisao da pessoa, " +
                    "o lobo e o carneiro ou o carneiro e o cesto de alface.";
            }
        }

        public int H
        {
            get
            {
                return h;
            }
        }

        public bool IsMeta
        {
            get
            {
                return (h == 0);
            }
        }

        private bool IsEstadoValido
        {
            get
            {
                if (lobo == MARGEM_FINAL && carneiro == MARGEM_FINAL && homem == MARGEM_INICIAL)
                {
                    // ha lobo e carneiro sem o homem na margem final ou
                    // ha carneiro e alface na margem final sem o homem
                    return false;
                }

                if (carneiro == MARGEM_FINAL && alface == MARGEM_FINAL && homem == MARGEM_INICIAL)
                {
                    // ha lobo e carneiro sem o homem na margem final ou
                    // ha carneiro e alface na margem final sem o homem
                    return false;
                }
                
                if (lobo == MARGEM_INICIAL && carneiro == MARGEM_INICIAL && homem == MARGEM_FINAL)
                {

                    // ha lobo e carneiro sem o homem na margem inicial ou
                    // ha carneiro e alface na margem inicial sem o homem
                    return false;
                }

                if (carneiro == MARGEM_INICIAL && alface == MARGEM_INICIAL && homem == MARGEM_FINAL)
                {

                    // ha lobo e carneiro sem o homem na margem inicial ou
                    // ha carneiro e alface na margem inicial sem o homem
                    return false;
                }

                return true;
            }
        }

        private EstadoHLAC TentarLevarOLobo()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL)
            {
                // tenta levar o lobo da margem inicial para a margem final
                if (lobo == MARGEM_INICIAL)
                {
                    novoEstado = new EstadoHLAC(MARGEM_FINAL, MARGEM_FINAL, carneiro, alface, "Leva o lobo para a margem final");
                }
            }
            else
            {
                // tenta levar o lobo da margem final para a margem inicial
                if (lobo == MARGEM_FINAL)
                {
                    novoEstado = new EstadoHLAC(MARGEM_INICIAL, MARGEM_INICIAL, carneiro, alface, "Leva o lobo para a margem inicial");
                }
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }

        private EstadoHLAC TentarLevarOCarneiro()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL)
            {
                // tenta levar o lobo da margem inicial para a margem final
                if (carneiro == MARGEM_INICIAL)
                {
                    novoEstado = new EstadoHLAC(MARGEM_FINAL, lobo, MARGEM_FINAL, alface, "Leva o carneiro para a margem final");
                }
            }
            else
            {
                // tenta levar o lobo da margem final para a margem inicial
                if (lobo == MARGEM_FINAL)
                {
                    novoEstado = new EstadoHLAC(MARGEM_INICIAL, lobo, MARGEM_INICIAL, alface, "Leva o carneiro para a margem inicial");
                }
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }

        private EstadoHLAC TentarLevarOAlface()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL)
            {
                // tenta levar o lobo da margem inicial para a margem final
                if (alface == MARGEM_INICIAL)
                {
                    novoEstado = new EstadoHLAC(MARGEM_FINAL, lobo, carneiro, MARGEM_FINAL, "Leva o alface para a margem final");
                }
            }
            else
            {
                // tenta levar o lobo da margem final para a margem inicial
                if (alface == MARGEM_FINAL)
                {
                    novoEstado = new EstadoHLAC(MARGEM_INICIAL, lobo, carneiro, MARGEM_INICIAL, "Leva o alface para a margem inicial");
                }
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }
        public IEnumerable<Estado> Sucessores
        {
            get
            {
                List<Estado> sucessores = new List<Estado>();

                // @@@

                EstadoHLAC novoEstado;

                // possíveis estados futuros:
                novoEstado = TentarLevarOLobo();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }

                novoEstado = TentarLevarOCarneiro();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }

                novoEstado = TentarLevarOAlface();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }
                return sucessores;
            }
        }
    }
}
