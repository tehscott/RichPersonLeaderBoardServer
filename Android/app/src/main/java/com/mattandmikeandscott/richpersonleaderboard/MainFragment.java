package com.mattandmikeandscott.richpersonleaderboard;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.widget.Space;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;

import java.util.ArrayList;
import java.util.List;

public class MainFragment extends Fragment {
    /**
     * The fragment argument representing the section number for this
     * fragment.
     */
    private static final String ARG_SECTION_NUMBER = "section_number";

    /**
     * Returns a new instance of this fragment for the given section
     * number.
     */
    public static MainFragment newInstance(int sectionNumber) {
        MainFragment fragment = new MainFragment();
        Bundle args = new Bundle();
        args.putInt(ARG_SECTION_NUMBER, sectionNumber);
        fragment.setArguments(args);
        return fragment;
    }

    public MainFragment() {}

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View rootView = null;
        int sectionNumber = getArguments().getInt(ARG_SECTION_NUMBER);

        switch (sectionNumber) {
            case 0:
                rootView = inflater.inflate(R.layout.fragment_all_time, container, false);

                ListView list = (ListView) rootView.findViewById(R.id.list);

                ArrayList<String[]> people = new ArrayList<>();
                for(int i = 0; i < 100; i++) {
                    people.add(new String[] { "Scott", "$100,000,000" });
                    people.add(new String[] { "Matt", "$100,000" });
                    people.add(new String[] { "Mike", "$100" });
                }

                list.setAdapter(new PersonListAdapter(getContext(), people));

                break;
            case 1:
                rootView = inflater.inflate(R.layout.fragment_today, container, false);
                break;
            case 2:
                rootView = inflater.inflate(R.layout.fragment_this_week, container, false);
                break;
            case 3:
                rootView = inflater.inflate(R.layout.fragment_this_month, container, false);
                break;
            case 4:
                rootView = inflater.inflate(R.layout.fragment_this_year, container, false);
                break;
        }

        return rootView;
    }
}