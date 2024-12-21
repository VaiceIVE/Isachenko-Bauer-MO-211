CREATE DATABASE transactional_outbox;

\connect transactional_outbox; 

CREATE TABLE "messages" (
  "id" serial PRIMARY KEY,
  "message" varchar NOT NULL,
  "is_sent" bool NOT NULL DEFAULT false
);
