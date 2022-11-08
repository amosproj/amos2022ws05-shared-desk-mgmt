--TODO: replace with actual database initialization
CREATE TABLE users (
	id int,
	name VARCHAR(50),
	password VARCHAR(50)
);

INSERT INTO users (id, name, password) VALUES (2, 'Dagobert2', 'money');
INSERT INTO users (id, name, password) VALUES (3, 'Donald2', 'quaack');