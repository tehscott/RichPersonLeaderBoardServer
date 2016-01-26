package com.mattandmikeandscott.richpersonleaderboard.domain;

public class Person {
    private int id;
    private String name;
    private double wealth;
    private int rank;

    public Person(int id, String name, double wealth, int rank) {
        this.id = id;
        this.name = name;
        this.wealth = wealth;
        this.rank = rank;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public double getWealth() {
        return wealth;
    }

    public void setWealth(double wealth) {
        this.wealth = wealth;
    }

    public int getRank() {
        return rank;
    }

    public void setRank(int rank) {
        this.rank = rank;
    }
}
