CREATE SCHEMA covid_result
    AUTHORIZATION app02;

CREATE TABLE covid_result.countrys
(
    id SERIAL PRIMARY KEY,
    Country character varying(50) NOT NULL,
    CountryCode character varying(50) NOT NULL,
    Province character varying(50) NOT NULL,
    City character varying(50) NOT NULL,
    CityCode character varying(50) NOT NULL,
    Lat character varying(50) NOT NULL,
    Lon character varying(50) NOT NULL,
    Confirmed character varying(100) NOT NULL,
    Deaths character varying(50) NOT NULL,
    Recovered character varying(50) NOT NULL,
    Active character varying(50) NOT NULL,
    Date character varying(50) NOT NULL
)
WITH (
    OIDS = FALSE
);

ALTER TABLE covid_result.countrys OWNER to app02;
