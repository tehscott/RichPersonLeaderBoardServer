package com.mattandmikeandscott.richpersonleaderboard;

import android.os.Bundle;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;
import com.mattandmikeandscott.richpersonleaderboard.network.Repository;

import java.util.ArrayList;

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
        View rootView = null;
        int sectionNumber = getArguments().getInt(ARG_SECTION_NUMBER);

        switch (sectionNumber) {
            case 0:
                rootView = inflater.inflate(R.layout.fragment_list, container, false);

                Message message = new Message();
                message.obj = true;
                message.what = MainActivity.HandlerResult.TOGGLE_LOADING.ordinal();
                ((MainActivity)getActivity()).handler.sendMessage(message);

                final View fragment = rootView;
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        Repository repository = new Repository();
                        ArrayList<Person> people = repository.getPeople(PeopleQueryType.AllTime);

                        ListView list = (ListView) fragment.findViewById(R.id.list);
                        Object[] parameters = new Object[] { getActivity(), list, people };

                        Message message = new Message();
                        message.what = MainActivity.HandlerResult.PERSON_INFO_AQUIRED.ordinal();
                        message.obj = parameters;

                        ((MainActivity)getActivity()).handler.sendMessage(message);
                    }
                }).start();

                break;
            case 1:
                rootView = inflater.inflate(R.layout.fragment_list, container, false);
                break;
            case 2:
                rootView = inflater.inflate(R.layout.fragment_list, container, false);
                break;
            case 3:
                rootView = inflater.inflate(R.layout.fragment_list, container, false);
                break;
            case 4:
                rootView = inflater.inflate(R.layout.fragment_list, container, false);
                break;
        }

        return rootView;
    }
}