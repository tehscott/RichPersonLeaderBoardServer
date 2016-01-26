package com.mattandmikeandscott.richpersonleaderboard.Network;

import android.content.res.Resources;
import android.util.Log;

import com.mattandmikeandscott.richpersonleaderboard.R;
import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;

import org.apache.http.HttpEntity;
import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpRequestBase;
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
import java.util.Arrays;
import java.util.List;

public class Repository {
    private Resources resources;

    public Repository(Resources resources) {
        this.resources = resources;
    }

    public ArrayList<Person> getPeople(PeopleQueryType peopleQueryType, int offset, int perPage) {
        String endpoint = "http://richpersonleaderboardserver.azurewebsites.net";

        if(peopleQueryType == PeopleQueryType.AllTime) {
            endpoint += "/api/persons";
        } else {
            throw new IllegalArgumentException("Invalid endpoint for getPeople()!");
        }

        List<String> parameterNames = Arrays.asList("offset", "perpage");
        List<String> parameterValues = Arrays.asList(String.valueOf(offset), String.valueOf(perPage));

        ArrayList<Person> people = new ArrayList<>();

        JSONArray peopleData = doGetRequest(endpoint, parameterNames, parameterValues);

        try {
            for(int i = 0; i < peopleData.length(); i++) {
                JSONObject jsonObject = peopleData.getJSONObject(i);

                people.add(new Person(jsonObject.getInt("PersonId"), jsonObject.getString("Name"), jsonObject.getDouble("Wealth"), i));
            }
        } catch(JSONException e) {
            Log.e(resources.getString(R.string.app_short_name), "Error parsing JSON data " + e.toString());

        }

        return people;
    }

    private JSONArray doGetRequest(String endpoint, List<String> parameterNames, List<String> parameterValues) {
        JSONArray jArray = new JSONArray();

        try {
            HttpGet httpGet = new HttpGet(endpoint + "?" + paramListToString(parameterNames, parameterValues));

            jArray = doRequest(httpGet);
        } catch(Exception e) {
            Log.e(resources.getString(R.string.app_short_name), "Error creating post request " + e.toString());
        }

        return jArray;
    }

    private JSONArray doPostRequest(String endpoint, ArrayList<NameValuePair> nameValuePairList) {
        JSONArray jArray = new JSONArray();

        try {
            HttpPost httpPost = new HttpPost(endpoint);
            httpPost.addHeader("User-Agent", "User-Agent");
            httpPost.setEntity(new UrlEncodedFormEntity(nameValuePairList, "UTF-8"));

            jArray = doRequest(httpPost);
        } catch(Exception e) {
            Log.e(resources.getString(R.string.app_short_name), "Error creating post request " + e.toString());
        }

        return jArray;
    }

    private JSONArray doRequest(HttpRequestBase request) {
        CloseableHttpClient client = HttpClients.createDefault();
        JSONArray jArray = new JSONArray();

        try {
            CloseableHttpResponse response = client.execute(request);

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
                Log.e(resources.getString(R.string.app_short_name), "Error converting result "+e.toString());
            }

            try {
                jArray = new JSONArray(result);
            } catch(JSONException e) {
                Log.e(resources.getString(R.string.app_short_name), "Error parsing JSON data " + e.toString());
            }
        } catch(Exception e) {

        } finally {
            try {
                client.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        return jArray;
    }

    private String paramListToString(List<String> parameterNames, List<String> parameterValues) {
        if(parameterNames.size() != parameterValues.size()) {
            throw new IllegalArgumentException("Parameter name and value array length does not match!");
        }

        String listAsString = "";

        for(int i = 0; i < parameterNames.size(); i++) {
            listAsString += parameterNames.get(i) + "=" + parameterValues.get(i) + "&"; // a trailing & is ok
        }

        return listAsString;
    }
}
