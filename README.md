=============================================================
1-Apresentação
=============================================================
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

=========================================================================================
2-Utilização 
=========================================================================================

Não foi realizado a implementação para implantação dos microserviços no docker, sendo utilizado
o docker para que as aplicações como o PostGree, RabbitMQ , MongoDb e Elasticsearch, Kibana fossem
utilizados, sendo que cada microserviço possui seu docker-compose.yml.

Recomendado para o uso que todos esses arquivos docker-compose sejam executados antes de realizar a execução
dos microserviços. Exemplo: docker-compose up

Utilizar a solution para subir todos os microserviços que possui a nomenclatura .Api.
e caso necessário ajustar os apontamentos no appsettings.json


