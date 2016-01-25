package com.mattandmikeandscott.richpersonleaderboard.domain;

public class Person {
    private int id;
    private String name;
    private double netWorth;
    private int rank;

    public Person(int id, String name, double netWorth, int rank) {
        this.id = id;
        this.name = name;
        this.netWorth = netWorth;
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

    public double getNetWorth() {
        return netWorth;
    }

    public void setNetWorth(double netWorth) {
        this.netWorth = netWorth;
    }

    public int getRank() {
        return rank;
    }

    public void setRank(int rank) {
        this.rank = rank;
    }
}
