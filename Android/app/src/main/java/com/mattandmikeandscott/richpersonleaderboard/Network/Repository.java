package com.mattandmikeandscott.richpersonleaderboard.network;

import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;

import java.util.ArrayList;

public class Repository {
    public ArrayList<Person> getPeople(PeopleQueryType peopleQueryType) {
        ArrayList<Person> people = new ArrayList<>();

        for(int i = 0; i < 100; i++) {
            people.add(new Person(i, "Scott" + i, (100000000 + i), i));
        }

        return people;
    }
}
