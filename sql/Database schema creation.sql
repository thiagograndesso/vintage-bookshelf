CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                                                       "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
                                                       "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Author" (
                          "Id" INTEGER NOT NULL CONSTRAINT "PK_Author" PRIMARY KEY AUTOINCREMENT,
                          "Name" VARCHAR(200) NOT NULL,
                          "BirthDate" DATETIMEOFFSET NOT NULL,
                          "Biography" VARCHAR(1000) NOT NULL
);

CREATE TABLE "Bookshelf" (
                             "Id" INTEGER NOT NULL CONSTRAINT "PK_Bookshelf" PRIMARY KEY AUTOINCREMENT,
                             "Name" VARCHAR(200) NOT NULL,
                             "Address" VARCHAR(200) NOT NULL,
                             "City" VARCHAR(200) NOT NULL
);

CREATE TABLE "Book" (
                        "Id" INTEGER NOT NULL CONSTRAINT "PK_Book" PRIMARY KEY AUTOINCREMENT,
                        "Title" VARCHAR(200) NOT NULL,
                        "Publisher" VARCHAR(200) NOT NULL,
                        "ReleaseYear" INTEGER NOT NULL,
                        "Summary" VARCHAR(1000) NOT NULL,
                        "AuthorId" INTEGER NOT NULL,
                        "BookshelfId" INTEGER NOT NULL,
                        CONSTRAINT "FK_Book_Author_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Author" ("Id") ON DELETE RESTRICT,
                        CONSTRAINT "FK_Book_Bookshelf_BookshelfId" FOREIGN KEY ("BookshelfId") REFERENCES "Bookshelf" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_Book_AuthorId" ON "Book" ("AuthorId");

CREATE INDEX "IX_Book_BookshelfId" ON "Book" ("BookshelfId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210131205214_Initial', '5.0.2');

COMMIT;
