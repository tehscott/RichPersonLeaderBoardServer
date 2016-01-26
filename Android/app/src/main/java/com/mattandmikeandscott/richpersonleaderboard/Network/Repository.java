package com.mattandmikeandscott.richpersonleaderboard.network;

import android.util.Log;

import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;

import org.apache.http.HttpEntity;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.impl.client.HttpClients;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;

public class Repository {
    public ArrayList<Person> getPeople(PeopleQueryType peopleQueryType) {
        ArrayList<Person> people = new ArrayList<>();

        CloseableHttpClient client = HttpClients.createDefault();
        try {
            HttpGet httpGet = new HttpGet("http://richpersonleaderboardserver.azurewebsites.net/leaderboard?offset=0&perpage=100");
            CloseableHttpResponse response = client.execute(httpGet);

            HttpEntity entity = response.getEntity();
            InputStream is = entity.getContent();

            String result = "";
            try {
                BufferedReader reader = new BufferedReader(new InputStreamReader(is,"iso-8859-1"),8);
                StringBuilder sb = new StringBuilder();
                String line = null;

                while ((line = reader.readLine()) != null) {
                    sb.append(line + "\n");
                }

                is.close();

                result = sb.toString();
            } catch(Exception e) {
                Log.e("RPLB", "Error converting result "+e.toString());
            }

            JSONArray jArray;
            try {
                jArray = new JSONArray(result);

                for(int i = 0; i < jArray.length(); i++) {
                    JSONObject jsonObject = jArray.getJSONObject(i);

                    people.add(new Person(jsonObject.getInt("PersonId"), jsonObject.getString("Name"), jsonObject.getDouble("Wealth"), i));
                }
            } catch(JSONException e) {
                Log.w("RPLB", "Results: "+result);
                Log.w("RPLB", "Error parsing JSON data "+e.toString());

            }
        } catch(Exception e) {

        } finally {
            try {
                client.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        return people;
    }
}
