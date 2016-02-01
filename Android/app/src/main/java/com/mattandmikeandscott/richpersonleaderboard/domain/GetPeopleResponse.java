package com.mattandmikeandscott.richpersonleaderboard.domain;

import android.content.Context;
import android.provider.Contacts;
import android.widget.ListView;

import java.util.ArrayList;

public class GetPeopleResponse {
    private Context context;
    private ListView list;
    private ArrayList<Person> people;
    private PeopleQueryType peopleQueryType;
    private RankType rankType;
    private boolean success;

    public GetPeopleResponse(Context context, ListView list, ArrayList<Person> people, PeopleQueryType peopleQueryType, RankType rankType, boolean success) {
        this.context = context;
        this.list = list;
        this.people = people;
        this.peopleQueryType = peopleQueryType;
        this.rankType = rankType;
        this.success = success;
    }

    public Context getContext() {
        return context;
    }

    public void setContext(Context context) {
        this.context = context;
    }

    public ListView getList() {
        return list;
    }

    public void setList(ListView list) {
        this.list = list;
    }

    public ArrayList<Person> getPeople() {
        return people;
    }

    public void setPeople(ArrayList<Person> people) {
        this.people = people;
    }

    public RankType getRankType() {
        return rankType;
    }

    public void setRankType(RankType rankType) {
        this.rankType = rankType;
    }

    public PeopleQueryType getPeopleQueryType() {
        return peopleQueryType;
    }

    public void setPeopleQueryType(PeopleQueryType peopleQueryType) {
        this.peopleQueryType = peopleQueryType;
    }

    public boolean getSuccess() {
        return success;
    }

    public void setSuccess(boolean success) {
        this.success = success;
    }
}
