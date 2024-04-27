CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Deliverers" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "CNPJ" character varying(14) NOT NULL,
    "BirthDate" timestamp with time zone NOT NULL,
    "CNHNumber" text NOT NULL,
    "CNHType" integer NOT NULL,
    "CNHImageUrl" text NOT NULL,
    CONSTRAINT "PK_Deliverers" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Deliverers_CNHNumber" ON "Deliverers" ("CNHNumber");

CREATE UNIQUE INDEX "IX_Deliverers_CNPJ" ON "Deliverers" ("CNPJ");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240420094639_InitialCreate', '8.0.4');

COMMIT;

