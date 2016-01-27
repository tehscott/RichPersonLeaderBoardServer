package com.mattandmikeandscott.richpersonleaderboard;

import android.os.Bundle;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;
import com.mattandmikeandscott.richpersonleaderboard.network.Repository;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.Map;

public class MainFragment extends Fragment {
    private static final String ARG_SECTION_NUMBER = "section_number";

    public static MainFragment newInstance(int sectionNumber) {
        MainFragment fragment = new MainFragment();
        Bundle args = new Bundle();
        args.putInt(ARG_SECTION_NUMBER, sectionNumber);
        fragment.setArguments(args);
        return fragment;
    }

    public MainFragment() {}

    @Override
    public View onCreateView(LayoutInflater inflater, final ViewGroup container, Bundle savedInstanceState) {
        View listLayout = null;
        int sectionNumber = getArguments().getInt(ARG_SECTION_NUMBER);

        switch (sectionNumber) {
            case 0:
                listLayout = inflater.inflate(R.layout.fragment_list, container, false);

                Map<String, Integer> parameters = new Hashtable<>();
                parameters.put("offset", 0);
                parameters.put("perpage", 100);

                refreshListAsync(listLayout, PeopleQueryType.AllTime, parameters);
                setupSwipeLayout(listLayout, PeopleQueryType.AllTime, parameters);
                setupButtons(listLayout);

                break;
            case 1:
                listLayout = inflater.inflate(R.layout.fragment_list, container, false);
                break;
            case 2:
                listLayout = inflater.inflate(R.layout.fragment_list, container, false);
                break;
            case 3:
                listLayout = inflater.inflate(R.layout.fragment_list, container, false);
                break;
            case 4:
                listLayout = inflater.inflate(R.layout.fragment_list, container, false);
                break;
        }

        return listLayout;
    }

    private void refreshListAsync(final View listLayout, final PeopleQueryType queryType, final Map<String, Integer> parameters) {
        final SwipeRefreshLayout swipeLayout = (SwipeRefreshLayout) listLayout.findViewById(R.id.list_container);
        swipeLayout.setRefreshing(true);
        swipeLayout.setEnabled(false);

        new Thread(new Runnable() {
            @Override
            public void run() {
                Repository repository = new Repository(getResources());
                ArrayList<Person> people = repository.getPeople(queryType, parameters);

                ListView list = (ListView) listLayout.findViewById(R.id.list);
                Object[] parameters = new Object[] { getActivity(), list, people };

                Message message = new Message();
                message.what = MainActivity.HandlerResult.PERSON_INFO_AQUIRED.ordinal();
                message.obj = parameters;

                ((MainActivity)getActivity()).handler.sendMessage(message);
            }
        }).start();
    }

    private void setupSwipeLayout(final View listLayout, final PeopleQueryType queryType, final Map<String, Integer> parameters) {
        final SwipeRefreshLayout swipeLayout = (SwipeRefreshLayout) listLayout.findViewById(R.id.list_container);
        swipeLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                refreshListAsync(listLayout, PeopleQueryType.AllTime, parameters);
            }
        });
    }

    private void setupButtons(final View listLayout) {
        listLayout.findViewById(R.id.find_me_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Map<String, Integer> parameters = new Hashtable<String, Integer>();
                parameters.put("id", 8);
                parameters.put("range", 5);

                refreshListAsync(listLayout, PeopleQueryType.Myself, parameters);
            }
        });
    }
}