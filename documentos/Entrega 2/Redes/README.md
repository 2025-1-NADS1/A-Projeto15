# Testes de Comunicação em Rede - Sistema de Casa Inteligente (C#)

## Objetivo

Implementar testes de comunicação em rede para verificar o funcionamento da troca de dados entre sensores (simulados), um controlador (simulado) e um servidor central (simulado) no sistema de casa inteligente. Os testes visam garantir que os dados dos sensores são enviados corretamente em formato JSON e que os comandos de controle textuais são recebidos e processados pelo servidor, utilizando uma interface interativa para selecionar qual dispositivo simular.

## Arquitetura de Teste

A simulação envolve duas aplicações C#:

* **SmartHomeSimulator (Servidor):** Uma aplicação de console que atua como o servidor central, escutando por conexões TCP na porta `12345` do endereço `127.0.0.1`. Ele recebe dados dos sensores em formato JSON e comandos textuais do controlador, respondendo com confirmações ou mensagens de erro. Além disso, esta aplicação agora inclui um servidor web para visualização dos dados dos sensores em tempo real.
* **SmartHomeDevices (Clientes Interativos):** Uma aplicação de console que apresenta um menu para simular individualmente diferentes sensores (Quarto 1, Quarto 2, Sala, Cozinha, Piscina) ou o Controlador. Após a simulação de um dispositivo, o usuário pode retornar ao menu principal para selecionar outro dispositivo.

## Ferramentas Utilizadas

* **C# (.NET):** Linguagem de programação utilizada para implementar a simulação.
* **System.Net.Sockets:** Biblioteca para comunicação TCP.
* **System.Text.Json:** Biblioteca para serialização e desserialização de dados JSON.
* **System.Threading:** Biblioteca para criar threads (embora neste formato interativo, o uso de múltiplas threads para os dispositivos na mesma instância foi simplificado para a interação via menu).
* **System.Net.HttpListener:** Biblioteca para implementar o servidor web (adicionado recentemente).

## Metodologia dos Testes

**Execução do Servidor:** Execute o projeto `SmartHomeSimulator`. Ele exibirá a mensagem "Servidor escutando em 127.0.0.1:12345" e "Servidor Web iniciado em http://localhost:8080".

**Execução dos Dispositivos Interativos:** Execute o projeto `SmartHomeDevices`. Um menu será exibido com as opções para simular diferentes sensores (1-5), o Controlador (6) ou Sair (7).

**Simulação de Sensores:**

1.  Escolha uma opção de sensor (1 a 5) no menu.
2.  A simulação do sensor conectará ao servidor.
3.  Pressione a tecla `ENTER` para enviar um conjunto de dados do sensor (timestamp, ID, temperatura, umidade, movimento) em formato JSON para o servidor.
4.  Digite `voltar` e pressione ENTER para retornar ao menu principal.
5.  Digite `sair` e pressione ENTER para encerrar a aplicação `SmartHomeDevices`.

**Simulação do Controlador:**

1.  Escolha a opção 6 (Controlador) no menu.
2.  A simulação do controlador conectará ao servidor.
3.  Digite comandos como `LIGAR Sala` ou `DESLIGAR Quarto 2` e pressione ENTER para enviá-los ao servidor.
4.  O servidor responderá com "OK" ou "Comando Desconhecido", que será exibido no console do controlador.
5.  Digite `voltar` e pressione ENTER para retornar ao menu principal.
6.  Digite `sair` e pressione ENTER para encerrar a aplicação `SmartHomeDevices`.

**Verificação no Servidor:** Observe a saída do servidor (`SmartHomeSimulator`) para confirmar o recebimento dos dados JSON dos sensores e dos comandos textuais do controlador. Verifique as respostas do servidor aos comandos.

**Retorno ao Menu:** Após simular um dispositivo, pressione ENTER na mensagem "Pressione [ENTER] para voltar ao menu principal." para retornar e selecionar outro dispositivo para testar.

**Sair:** Escolha a opção 7 no menu principal para encerrar a aplicação `SmartHomeDevices`.

## Critérios de Sucesso/Falha

**Envio de Dados dos Sensores:**

* **Sucesso:** O servidor recebe dados formatados em JSON contendo os campos esperados de cada sensor simulado. A `ID_Sensor` deve corresponder ao sensor simulado. Os dados também são armazenados para visualização na interface web.
* **Falha:** Dados ausentes, formato JSON inválido no servidor, ou `ID_Sensor` incorreta.

**Recebimento de Comandos do Controlador:**

* **Sucesso:** O servidor recebe comandos textuais iniciados com `LIGAR` ou `DESLIGAR` seguido do nome ou ID do cômodo e responde com "OK".
* **Falha:** Comandos válidos não são reconhecidos, o servidor não responde ou retorna uma mensagem de erro para comandos válidos.

**Tratamento de Comando Desconhecido:**

* **Sucesso:** O servidor responde com "Comando Desconhecido." para comandos não reconhecidos.

**Navegação no Menu:**

* **Sucesso:** O usuário consegue selecionar diferentes dispositivos para simular e retornar ao menu principal usando a opção "voltar".
* **Falha:** A opção "voltar" não funciona corretamente, ou a navegação no menu é interrompida.

## Resultados dos Testes

Ao executar a simulação interativa, os seguintes resultados foram observados:

* O servidor (`SmartHomeSimulator`) iniciou e aguardou conexões na porta 12345. O servidor web também iniciou na porta 8080.
* Ao executar o cliente interativo (`SmartHomeDevices`), o menu principal foi exibido corretamente, permitindo a seleção de diferentes dispositivos.
* A simulação individual de cada sensor (opções 1 a 5) estabeleceu conexão com o servidor. Ao pressionar ENTER, dados JSON representando as leituras do sensor foram enviados com sucesso e visualizados na saída do servidor e na interface web. A `ID_Sensor` em cada mensagem correspondia ao sensor simulado. A opção "voltar" permitiu retornar ao menu principal.
* A simulação do Controlador (opção 6) estabeleceu conexão com o servidor. Comandos como "LIGAR Sala" e "DESLIGAR Quarto 1" foram enviados com sucesso e o servidor respondeu com "OK: ...", que foi exibido no console do controlador. O comando desconhecido "ABRIR PORTA" resultou na resposta "Comando Desconhecido." do servidor. A opção "voltar" permitiu retornar ao menu principal.
* A opção "Sair" (opção 7) encerrou a aplicação `SmartHomeDevices`.

## Interpretação dos Resultados

Os testes demonstram que a comunicação TCP básica entre os sensores simulados, o controlador simulado e o servidor central está funcionando conforme o esperado. Os dados dos sensores são enviados corretamente em formato JSON, e os comandos de controle textuais são recebidos e processados pelo servidor com respostas adequadas. A interface interativa do `SmartHomeDevices` facilita a verificação da comunicação de diferentes componentes do sistema de forma isolada e sequencial.

## Com o Novo Servidor Web para Visualização de Dados

Recentemente, o `SmartHomeSimulator` foi aprimorado com uma interface web para a visualização dos dados dos sensores em tempo real diretamente no seu navegador.

### Como Acessar a Interface Web

1.  **Execute o Servidor:** Certifique-se de que o projeto `SmartHomeSimulator` esteja em execução. Ele iniciará dois servidores: um para comunicação TCP na porta `12345` e outro para a interface web na porta `8080`. Você verá a mensagem "Servidor Web iniciado em http://localhost:8080" no console.
2.  **Abra o Navegador:** Utilize seu navegador web preferido.
3.  **Acesse o Endereço:** Digite o seguinte endereço na barra de endereços do navegador e pressione Enter: `http://localhost:8080/`

### Funcionalidades da Interface Web

* **Visualização Dinâmica:** A página web se atualiza automaticamente a cada 2 segundos (`<meta http-equiv='refresh' content='2'>`), exibindo os dados mais recentes dos sensores assim que são recebidos e processados pelo servidor.
* **Organização Clara:** Os dados de cada sensor, formatados em JSON, são apresentados em blocos distintos (`<div class='box'>`), facilitando a identificação e leitura dos diferentes parâmetros (timestamp, ID, temperatura, umidade, movimento, etc.).
* **Formato de Dados:** Dentro de cada bloco, os pares chave-valor do JSON são exibidos em linhas separadas, tornando a informação ainda mais acessível.
* **Tratamento de Dados Não JSON:** Caso o servidor receba dados que não estejam no formato JSON esperado, eles serão exibidos dentro de um bloco `<pre>`, preservando a informação para análise.
* **Histórico Recente:** A interface web mantém um histórico dos últimos 50 conjuntos de dados recebidos (`if (sensorData.Count > 50) sensorData.RemoveAt(0);`), permitindo acompanhar a evolução das leituras dos sensores ao longo do tempo.

Agora, além de verificar a comunicação via console, você tem uma interface web intuitiva para monitorar os dados do seu sistema de casa inteligente em tempo real!