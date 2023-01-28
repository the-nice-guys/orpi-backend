.PHONY: up
up:
	docker-compose up -d
	make create-topic

.PHONY: create-topic
create-topic:
	docker-compose exec broker kafka-topics --create --topic massages --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1 --if-not-exists


.PHONY: stop
stop:
	docker-compose  stop

.PHONY: down
down:
	docker-compose down

.PHONY: broker-bash
broker-bash:
	docker exec -it broker bash


.PHONY: zookeeper-bash
zookeeper-bash:
	docker exec -it zookeeper bash

.PHONY: topic-list
topic-list:
	docker-compose -f docker-compose.kafka.yml exec broker kafka-topics --list --bootstrap-server localhost:9092