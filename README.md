Esta aplicação visa atender o ecosistema de locação de motos.

É composta por um conjunto de 4 microserviços:
- Autenticação
- Motos
- Entregadores
- Locação

- Autenticação
    Utilizada para realizar a autenticação e emissão de token

- Motos
    Responsável pela manutenção do cadastro de motos.

- Entregadores
    Responsável pela manutenção do cadastro de entregadores.

- Locação de Motos
    Responsável pela gerenciamento de locação de motos.

  A comunicação entre os microserviços quando realizada é por meio
  de utilização de requisições rest de modo síncrono.

  A única utilização de mensageria foi no cadastro de motos, utilizando-se
  do RabbitMQ . Foi desenvolvido um background service que faz o papel de Consumer
  processando o Evento de cadastro de motos.
