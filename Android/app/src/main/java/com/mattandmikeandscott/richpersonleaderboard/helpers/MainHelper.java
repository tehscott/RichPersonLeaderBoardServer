package com.mattandmikeandscott.richpersonleaderboard.helpers;

import android.support.v4.view.ViewPager;
import android.support.v7.app.ActionBar;
import android.view.View;
import android.widget.Toast;

import com.google.android.gms.auth.api.Auth;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.api.GoogleApiClient;
import com.mattandmikeandscott.richpersonleaderboard.MainActivity;
import com.mattandmikeandscott.richpersonleaderboard.MainFragment;
import com.mattandmikeandscott.richpersonleaderboard.R;
import com.mattandmikeandscott.richpersonleaderboard.adapters.SectionsPagerAdapter;
import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.RankType;

import java.util.Hashtable;
import java.util.Map;

public class MainHelper {
    private MainActivity mainActivity;

    public MainHelper(MainActivity mainActivity) {
        this.mainActivity = mainActivity;
    }

    public void setupButtons() {
        mainActivity.findViewById(R.id.find_me_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Toast.makeText(mainActivity, "Searching with hard coded ID of 1", Toast.LENGTH_SHORT).show();
                findMe();
            }
        });
    }

    public void setupViewPager() {
        mainActivity.setSectionsPagerAdapter(new SectionsPagerAdapter(mainActivity.getSupportFragmentManager()));
        mainActivity.setViewPager((ViewPager) mainActivity.findViewById(R.id.pager));
        mainActivity.getViewPager().setAdapter(mainActivity.getSectionsPagerAdapter());
        mainActivity.getViewPager().setOnPageChangeListener(new ViewPager.SimpleOnPageChangeListener() {
            @Override
            public void onPageSelected(int position) {
                mainActivity.getSupportActionBar().setSelectedNavigationItem(position);
            }
        });

        for (int i = 0; i < mainActivity.getSectionsPagerAdapter().getCount(); i++) {
            mainActivity.getSupportActionBar().addTab(
                    mainActivity.getSupportActionBar().newTab()
                            .setText(mainActivity.getSectionsPagerAdapter().getPageTitle(i))
                            //.setIcon(mSectionsPagerAdapter.getPageIcon(i))
                            .setTabListener(mainActivity));
        }
    }

    private void findMe() {
        Map<String, Integer> parameters = new Hashtable<>();
        parameters.put("id", Integer.valueOf(mainActivity.getSignInHelper().getId()));
        parameters.put("range", 5);

        int currentPosition = mainActivity.getViewPager().getCurrentItem();
        String tag = "";
        RankType rankType = RankType.AllTime;

        switch(currentPosition) {
            case 0:
                rankType = RankType.AllTime;
                tag = rankType.getName();
                break;
            case 1:
                rankType = RankType.Day;
                tag = rankType.getName();
                break;
            case 2:
                rankType = RankType.Week;
                tag = rankType.getName();
                break;
            case 3:
                rankType = RankType.Month;
                tag = rankType.getName();
                break;
            case 4:
                rankType = RankType.Year;
                tag = rankType.getName();
                break;
        }

        View fragmentList = mainActivity.getViewPager().findViewWithTag(tag);
        MainFragment.refreshListAsync(mainActivity.getResources(), mainActivity, fragmentList, PeopleQueryType.Myself, rankType, parameters);
    }

    public void setupActionBar() {
        // Set up the action bar.
        final ActionBar actionBar = mainActivity.getSupportActionBar();
        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);

        hideActionBarTitle(actionBar);
    }

    private void hideActionBarTitle(ActionBar actionBar) {
        actionBar.setDisplayShowTitleEnabled(false);
        actionBar.setDisplayShowHomeEnabled(false);
    }
}
