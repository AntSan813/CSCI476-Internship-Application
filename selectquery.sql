use Data;
GO

drop table Forms;
GO
drop table Templates;
GO
/*holds questions and answers*/
/*each row is a student's form*/
/*signatures held in each ques json*/

CREATE TABLE Forms (
	id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	created_at DATETIME NOT NULL DEFAULT GETDATE(),
	updated_at DATETIME NOT NULL DEFAULT GETDATE(),
	deleted_at DATETIME,

	wu_id VARCHAR(30) NOT NULL, /*or maybe email from sign in page?*/

	student_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	employer_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	faculty_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	student_services_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	administrator_questions VARCHAR(1000) NOT NULL DEFAULT '{}',

	template_id INT NOT NULL,
);
GO


/*holds initial questions*/
/*signatures held in each ques json*/

CREATE TABLE Templates (
 	id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
 	created_at DATETIME NOT NULL DEFAULT GETDATE(),
 	updated_at DATETIME NOT NULL DEFAULT GETDATE(),
 	deleted_at DATETIME,

	name VARCHAR(256) NOT NULL,
	student_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	employer_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	faculty_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	student_services_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	administrator_questions VARCHAR(1000) NOT NULL DEFAULT '{}',
	is_active BIT NOT NULL DEFAULT 0,
);
GO

/*Insert into Form Table*/
INSERT INTO Forms (wu_id, student_questions,employer_questions,faculty_questions,student_services_questions,administrator_questions,template_id)
VALUES ('W30267183','{"Today''s date":"","Class enrolled in":"","Semester/Year Enrolled":"","Intern Name":"","Winthrop email":"","Phone number":"","Student ID #":"","Class/Graduation Year":"","Major/Concentration":"","Are you legally authorized to hold a paid off-campus internship in the U.S.?":0,"I have read the agreement and will fulfill the duties and responsibilities outlined for the internship and the academic requirements for completing the internship course for credit. ":{"signature":{"signed":0,"date":""}},"Internship program guidelines (long paragraph)":{"signature":{"signed":0,"date":""}},"Professional Conduct Expectations":{"signature":{"signed":0,"date":""}},"Explain how this internship will add to your educational experience at Winthrop University":"","What professoinal and personal goals do you hope to achieve while at this internship?":""}',
'{"For profit":0,"Organization Name":"","Business License # or FEIN #":"","Statue issued":"","Direct Internship Supervisor":"","Supervisors Title":"","Physical Address":"","Available for site visit?":0,"Supervisor Phone":"","Supervisor Email":"","Internship Projected Start Date":"","Internship Projected End Date":"","Est. total number of weeks":"","est. total hours/week":"","paid":{"Yes?":0,"No?":0,"If yes, $":""},"Additional compensation/stipend":"","Intern Tasks/Role":"","Specific Projects Intern will work on/assist with":"","Learning Outcomes for Intern":"","Additional comments regarding internship":"","I approve of and agree to the Learning Agreement. I agree to abide by all the Equal Opportunity/Affirmative Action and other related federal and state laws and regulations in the hiring of Winthrop University students. I agree that the company will instruct/orient the student on company policies/procedures, and provide a safe working environment":{"signature":{"signed":0,"date":""}}}',
'{"Listed below are specific assignments that will be required of all students completing an internship in order to satisfactorily complete the experience and receive academic credit. Please indicate any other assignments that will be required during this work experience: Required: 1. Learning Journal 4. Final Report and Presentation 2. Evaluation of Employer (3/Internship) 5. Final Evaluation of Internship 3. Participation in Site Visit (if possible) 6. Documentation of Hours":{"signature":{"signed":0,"date":""}},"This student meets academic standards and prerequisites of the internship program and therefore has my approval to participate":{"signature":{"signed":0,"date":""}}}',
'{"This student meets academic standards and prerequisites of the internship program and therefore has my approval to participate":{"signature":{"signed":0,"date":""}},"Class meets requirements":0,"GPA req met":0,"meets POS req (MBA)":0}',
'{"Listed below are specific assignments that will be required of all students completing an internship in order to satisfactorily complete the experience and receive academic credit. Please indicate any other assignments that will be required during this work experience: Required: 1. Learning Journal 4. Final Report and Presentation 2. Evaluation of Employer (3/Internship) 5. Final Evaluation of Internship 3. Participation in Site Visit (if possible) 6. Documentation of Hours":{"signature":{"signed":0,"date":""}}}',
1);
GO

INSERT INTO Forms (wu_id, student_questions,employer_questions,faculty_questions,student_services_questions,administrator_questions,template_id)
VALUES ('W30267283','{"Today''s date":"","Class enrolled in":"","Semester/Year Enrolled":"","Intern Name":"","Winthrop email":"","Phone number":"","Student ID #":"","Class/Graduation Year":"","Major/Concentration":"","Are you legally authorized to hold a paid off-campus internship in the U.S.?":0,"I have read the agreement and will fulfill the duties and responsibilities outlined for the internship and the academic requirements for completing the internship course for credit. ":{"signature":{"signed":0,"date":""}},"Internship program guidelines (long paragraph)":{"signature":{"signed":0,"date":""}},"Professional Conduct Expectations":{"signature":{"signed":0,"date":""}},"Explain how this internship will add to your educational experience at Winthrop University":"","What professoinal and personal goals do you hope to achieve while at this internship?":""}',
'{"For profit":0,"Organization Name":"","Business License # or FEIN #":"","Statue issued":"","Direct Internship Supervisor":"","Supervisors Title":"","Physical Address":"","Available for site visit?":0,"Supervisor Phone":"","Supervisor Email":"","Internship Projected Start Date":"","Internship Projected End Date":"","Est. total number of weeks":"","est. total hours/week":"","paid":{"Yes?":0,"No?":0,"If yes, $":""},"Additional compensation/stipend":"","Intern Tasks/Role":"","Specific Projects Intern will work on/assist with":"","Learning Outcomes for Intern":"","Additional comments regarding internship":"","I approve of and agree to the Learning Agreement. I agree to abide by all the Equal Opportunity/Affirmative Action and other related federal and state laws and regulations in the hiring of Winthrop University students. I agree that the company will instruct/orient the student on company policies/procedures, and provide a safe working environment":{"signature":{"signed":0,"date":""}}}',
'{"Listed below are specific assignments that will be required of all students completing an internship in order to satisfactorily complete the experience and receive academic credit. Please indicate any other assignments that will be required during this work experience: Required: 1. Learning Journal 4. Final Report and Presentation 2. Evaluation of Employer (3/Internship) 5. Final Evaluation of Internship 3. Participation in Site Visit (if possible) 6. Documentation of Hours":{"signature":{"signed":0,"date":""}},"This student meets academic standards and prerequisites of the internship program and therefore has my approval to participate":{"signature":{"signed":0,"date":""}}}',
'{"This student meets academic standards and prerequisites of the internship program and therefore has my approval to participate":{"signature":{"signed":0,"date":""}},"Class meets requirements":0,"GPA req met":0,"meets POS req (MBA)":0}',
'{"Listed below are specific assignments that will be required of all students completing an internship in order to satisfactorily complete the experience and receive academic credit. Please indicate any other assignments that will be required during this work experience: Required: 1. Learning Journal 4. Final Report and Presentation 2. Evaluation of Employer (3/Internship) 5. Final Evaluation of Internship 3. Participation in Site Visit (if possible) 6. Documentation of Hours":{"signature":{"signed":0,"date":""}}}',
1);
GO
INSERT INTO Templates (name, student_questions,employer_questions,faculty_questions,student_services_questions,administrator_questions, is_active)
VALUES ('Fall2018','{"Today''s date":"","Class enrolled in":"","Semester/Year Enrolled":"","Intern Name":"","Winthrop email":"","Phone number":"","Student ID #":"","Class/Graduation Year":"","Major/Concentration":"","Are you legally authorized to hold a paid off-campus internship in the U.S.?":0,"I have read the agreement and will fulfill the duties and responsibilities outlined for the internship and the academic requirements for completing the internship course for credit. ":{"signature":{"signed":0,"date":""}},"Internship program guidelines (long paragraph)":{"signature":{"signed":0,"date":""}},"Professional Conduct Expectations":{"signature":{"signed":0,"date":""}},"Explain how this internship will add to your educational experience at Winthrop University":"","What professoinal and personal goals do you hope to achieve while at this internship?":""}',
'{"For profit":0,"Organization Name":"","Business License # or FEIN #":"","Statue issued":"","Direct Internship Supervisor":"","Supervisors Title":"","Physical Address":"","Available for site visit?":0,"Supervisor Phone":"","Supervisor Email":"","Internship Projected Start Date":"","Internship Projected End Date":"","Est. total number of weeks":"","est. total hours/week":"","paid":{"Yes?":0,"No?":0,"If yes, $":""},"Additional compensation/stipend":"","Intern Tasks/Role":"","Specific Projects Intern will work on/assist with":"","Learning Outcomes for Intern":"","Additional comments regarding internship":"","I approve of and agree to the Learning Agreement. I agree to abide by all the Equal Opportunity/Affirmative Action and other related federal and state laws and regulations in the hiring of Winthrop University students. I agree that the company will instruct/orient the student on company policies/procedures, and provide a safe working environment":{"signature":{"signed":0,"date":""}}}',
'{"Listed below are specific assignments that will be required of all students completing an internship in order to satisfactorily complete the experience and receive academic credit. Please indicate any other assignments that will be required during this work experience: Required: 1. Learning Journal 4. Final Report and Presentation 2. Evaluation of Employer (3/Internship) 5. Final Evaluation of Internship 3. Participation in Site Visit (if possible) 6. Documentation of Hours":{"signature":{"signed":0,"date":""}},"This student meets academic standards and prerequisites of the internship program and therefore has my approval to participate":{"signature":{"signed":0,"date":""}}}',
'{"This student meets academic standards and prerequisites of the internship program and therefore has my approval to participate":{"signature":{"signed":0,"date":""}},"Class meets requirements":0,"GPA req met":0,"meets POS req (MBA)":0}',
'{"Listed below are specific assignments that will be required of all students completing an internship in order to satisfactorily complete the experience and receive academic credit. Please indicate any other assignments that will be required during this work experience: Required: 1. Learning Journal 4. Final Report and Presentation 2. Evaluation of Employer (3/Internship) 5. Final Evaluation of Internship 3. Participation in Site Visit (if possible) 6. Documentation of Hours":{"signature":{"signed":0,"date":""}}}',
1);
GO
