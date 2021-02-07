-- Generate Initial Library Branches
INSERT INTO library_branches
	("Name", "Address", "Telephone", "Description", "OpenDate", "ImageUrl")
VALUES
	('Downtown', '172 Union St', '555-432-5567', 'Large historical library located in the heart of downtown', '1937-05-19', 'lib_downtown.png'),
	('Pacific Branch', '1881 Evergreen St', '555-432-3516', 'Seaside neighborhood library branch', '1980-08-01', 'lib_pacific.png'),
	('Oakville', '1077 Enterprise Blvd', '555-112-3491', 'Neighborhood library serving Oakville residents', '1992-03-22', 'lib_oakville.png'),
	('Centerville University Medical', '16 University Ave', '555-432-5567', 'Medical library serving the Centerville University', '1952-08-12', 'lib_medical.png'),
	('Westfield', '35 Dover Ave', '555-653-9719', 'The Westfield regional branch contains the largest collection of fiction in the country'', '2010-01-06', 'lib_westfield.png');
