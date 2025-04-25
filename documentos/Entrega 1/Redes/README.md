# Testes de Comunicação em Rede - Sistema de Casa Inteligente (C#)

## Objetivo

Implementar testes de comunicação em rede para verificar o funcionamento da troca de dados entre sensores (simulados), um controlador (simulado) e um servidor central (simulado) no sistema de casa inteligente. Os testes visam garantir que os dados dos sensores são enviados corretamente em formato JSON e que os comandos de controle textuais são recebidos e processados pelo servidor, utilizando uma interface interativa para selecionar qual dispositivo simular.

## Arquitetura de Teste

A simulação envolve duas aplicações C#:

1.  **`SmartHomeSimulator` (Servidor):** Uma aplicação de console que atua como o servidor central, escutando por conexões TCP na porta `12345` do endereço `127.0.0.1`. Ele recebe dados dos sensores em formato JSON e comandos textuais do controlador, respondendo com confirmações ou mensagens de erro.
2.  **`SmartHomeDevices` (Clientes Interativos):** Uma aplicação de console que apresenta um menu para simular individualmente diferentes sensores (Quarto 1, Quarto 2, Sala, Cozinha, Piscina) ou o Controlador. Após a simulação de um dispositivo, o usuário pode retornar ao menu principal para selecionar outro dispositivo.

## Ferramentas Utilizadas

* **C# (.NET):** Linguagem de programação utilizada para implementar a simulação.
* **`System.Net.Sockets`:** Biblioteca para comunicação TCP.
* **`System.Text.Json`:** Biblioteca para serialização e desserialização de dados JSON.
* **`System.Threading`:** Biblioteca para criar threads (embora neste formato interativo, o uso de múltiplas threads para os dispositivos na mesma instância foi simplificado para a interação via menu).

## Metodologia dos Testes

1.  **Execução do Servidor:** Execute o projeto `SmartHomeSimulator`. Ele exibirá a mensagem "Servidor escutando em 127.0.0.1:12345".
2.  **Execução dos Dispositivos Interativos:** Execute o projeto `SmartHomeDevices`. Um menu será exibido com as opções para simular diferentes sensores (1-5), o Controlador (6) ou Sair (7).
3.  **Simulação de Sensores:**
    * Escolha uma opção de sensor (1 a 5) no menu.
    * A simulação do sensor conectará ao servidor.
    * Pressione a tecla **ENTER** para enviar um conjunto de dados do sensor (timestamp, ID, temperatura, umidade, movimento) em formato JSON para o servidor.
    * Digite `voltar` e pressione ENTER para retornar ao menu principal.
    * Digite `sair` e pressione ENTER para encerrar a aplicação `SmartHomeDevices`.
4.  **Simulação do Controlador:**
    * Escolha a opção 6 (Controlador) no menu.
    * A simulação do controlador conectará ao servidor.
    * Digite comandos como `LIGAR Sala` ou `DESLIGAR Quarto 2` e pressione ENTER para enviá-los ao servidor.
    * O servidor responderá com "OK" ou "Comando Desconhecido", que será exibido no console do controlador.
    * Digite `voltar` e pressione ENTER para retornar ao menu principal.
    * Digite `sair` e pressione ENTER para encerrar a aplicação `SmartHomeDevices`.
5.  **Verificação no Servidor:** Observe a saída do servidor (`SmartHomeSimulator`) para confirmar o recebimento dos dados JSON dos sensores e dos comandos textuais do controlador. Verifique as respostas do servidor aos comandos.
6.  **Retorno ao Menu:** Após simular um dispositivo, pressione ENTER na mensagem "Pressione [ENTER] para voltar ao menu principal." para retornar e selecionar outro dispositivo para testar.
7.  **Sair:** Escolha a opção 7 no menu principal para encerrar a aplicação `SmartHomeDevices`.

## Critérios de Sucesso/Falha

* **Envio de Dados dos Sensores:**
    * **Sucesso:** O servidor recebe dados formatados em JSON contendo os campos esperados de cada sensor simulado. A `ID_Sensor` deve corresponder ao sensor simulado.
    * **Falha:** Dados ausentes, formato JSON inválido no servidor, ou `ID_Sensor` incorreta.
* **Recebimento de Comandos do Controlador:**
    * **Sucesso:** O servidor recebe comandos textuais iniciados com `LIGAR` ou `DESLIGAR` seguido do nome ou ID do cômodo e responde com "OK".
    * **Falha:** Comandos válidos não são reconhecidos, o servidor não responde ou retorna uma mensagem de erro para comandos válidos.
* **Tratamento de Comando Desconhecido:**
    * **Sucesso:** O servidor responde com "Comando Desconhecido." para comandos não reconhecidos.
* **Navegação no Menu:**
    * **Sucesso:** O usuário consegue selecionar diferentes dispositivos para simular e retornar ao menu principal usando a opção "voltar".
    * **Falha:** A opção "voltar" não funciona corretamente, ou a navegação no menu é interrompida.

## Resultados dos Testes

Ao executar a simulação interativa, os seguintes resultados foram observados:

* O servidor (`SmartHomeSimulator`) iniciou e aguardou conexões na porta 12345.
* Ao executar o cliente interativo (`SmartHomeDevices`), o menu principal foi exibido corretamente, permitindo a seleção de diferentes dispositivos.
* A simulação individual de cada sensor (opções 1 a 5) estabeleceu conexão com o servidor. Ao pressionar ENTER, dados JSON representando as leituras do sensor foram enviados com sucesso e visualizados na saída do servidor. A `ID_Sensor` em cada mensagem correspondia ao sensor simulado. A opção "voltar" permitiu retornar ao menu principal.
* A simulação do Controlador (opção 6) estabeleceu conexão com o servidor. Comandos como "LIGAR Sala" e "DESLIGAR Quarto 1" foram enviados com sucesso e o servidor respondeu com "OK: ...", que foi exibido no console do controlador. O comando desconhecido "ABRIR PORTA" resultou na resposta "Comando Desconhecido." do servidor. A opção "voltar" permitiu retornar ao menu principal.
* A opção "Sair" (opção 7) encerrou a aplicação `SmartHomeDevices`.

## Interpretação dos Resultados

Os testes demonstram que a comunicação TCP básica entre os sensores simulados, o controlador simulado e o servidor central está funcionando conforme o esperado. Os dados dos sensores são enviados corretamente em formato JSON, e os comandos de controle textuais são recebidos e processados pelo servidor com respostas adequadas. A interface interativa do `SmartHomeDevices` facilita a verificação da comunicação de diferentes componentes do sistema de forma isolada e sequencial.

## Possíveis Melhorias ou Próximos Passos

* Implementar um tratamento de erros mais robusto nas conexões e na troca de dados.
* Adicionar mais detalhes à simulação dos sensores (por exemplo, variação de temperatura e umidade ao longo do tempo, simulação de movimento).
* Expandir os comandos de controle para elementos individuais dentro de cada cômodo e incluir respostas mais detalhadas do servidor sobre o estado dos atuadores.
* Considerar a implementação de um protocolo de comunicação mais estruturado em vez de texto simples para maior robustez e extensibilidade.
* Adicionar testes de latência e simulação de perda de pacotes para avaliar o desempenho e a resiliência da comunicação em condições de rede adversas.