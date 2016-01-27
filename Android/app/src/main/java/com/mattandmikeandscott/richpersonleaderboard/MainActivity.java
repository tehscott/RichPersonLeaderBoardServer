package com.mattandmikeandscott.richpersonleaderboard;

import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.ListView;

import com.mattandmikeandscott.richpersonleaderboard.Adapters.PersonListAdapter;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;

import java.util.ArrayList;

import static com.mattandmikeandscott.richpersonleaderboard.MainActivity.HandlerResult.*;

public class MainActivity extends ActionBarActivity implements ActionBar.TabListener {
    private SectionsPagerAdapter mSectionsPagerAdapter;

    private ViewPager mViewPager;

    public enum HandlerResult {
        PERSON_INFO_AQUIRED,
        TOGGLE_LOADING
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Set up the action bar.
        final ActionBar actionBar = getSupportActionBar();
        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);

        hideActionBarTitle(actionBar);

        // Create the adapter that will return a fragment for each of the three
        // primary sections of the activity.
        mSectionsPagerAdapter = new SectionsPagerAdapter(getSupportFragmentManager());

        // Set up the ViewPager with the sections adapter.
        mViewPager = (ViewPager) findViewById(R.id.pager);
        mViewPager.setAdapter(mSectionsPagerAdapter);

        // When swiping between different sections, select the corresponding
        // tab. We can also use ActionBar.Tab#select() to do this if we have
        // a reference to the Tab.
        mViewPager.setOnPageChangeListener(new ViewPager.SimpleOnPageChangeListener() {
            @Override
            public void onPageSelected(int position) {
                actionBar.setSelectedNavigationItem(position);
            }
        });

        // For each of the sections in the app, add a tab to the action bar.
        for (int i = 0; i < mSectionsPagerAdapter.getCount(); i++) {
            // Create a tab with text corresponding to the page title defined by
            // the adapter. Also specify this Activity object, which implements
            // the TabListener interface, as the callback (listener) for when
            // this tab is selected.
            actionBar.addTab(
                    actionBar.newTab()
                            .setText(mSectionsPagerAdapter.getPageTitle(i))
                            //.setIcon(mSectionsPagerAdapter.getPageIcon(i))
                            .setTabListener(this));
        }
    }

    private void hideActionBarTitle(ActionBar actionBar) {
        actionBar.setDisplayShowTitleEnabled(false);
        actionBar.setDisplayShowHomeEnabled(false);
    }

    @Override
    public void onTabSelected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
        // When the given tab is selected, switch to the corresponding page in
        // the ViewPager.
        mViewPager.setCurrentItem(tab.getPosition());
    }

    @Override
    public void onTabUnselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
    }

    @Override
    public void onTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
    }

    /**
     * A {@link FragmentPagerAdapter} that returns a fragment corresponding to
     * one of the sections/tabs/pages.
     */
    public class SectionsPagerAdapter extends FragmentPagerAdapter {
        public SectionsPagerAdapter(FragmentManager fm) {
            super(fm);
        }

        @Override
        public Fragment getItem(int position) {
            // getItem is called to instantiate the fragment for the given page.
            // Return a MainFragment (defined as a static inner class below).
            return MainFragment.newInstance(position);
        }

        @Override
        public int getCount() {
            return 5;
        }

        @Override
        public CharSequence getPageTitle(int position) {
            switch (position) {
                case 0:
                    return "ALL TIME";
                case 1:
                    return "TODAY";
                case 2:
                    return "THIS WEEK";
                case 3:
                    return "THIS MONTH";
                case 4:
                    return "THIS YEAR";
                default:
                    return "";
            }
        }

        //public int getPageIcon(int position) {
        //    switch (position) {
        //        case 0:
        //            return R.drawable.table_icon;
        //        case 1:
        //            return R.drawable.boss_icon;
        //        case 2:
        //            return R.drawable.home_icon;
        //        case 3:
        //            return R.drawable.cards_icon;
        //        default:
        //            return -1;
        //    }
        //}
    }

    public Handler handler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage(msg);

            if(msg.what == TOGGLE_LOADING.ordinal()) {
                boolean isVisible = (boolean) msg.obj;

                LinearLayout progressBar = (LinearLayout) findViewById(R.id.progress_container);
                progressBar.setVisibility(isVisible ? View.VISIBLE : View.GONE);
            } else if (msg.what == PERSON_INFO_AQUIRED.ordinal()) {
                Context context = (Context) ((Object[]) msg.obj)[0];
                ListView list = (ListView) ((Object[]) msg.obj)[1];
                ArrayList<Person> people = (ArrayList<Person>) ((Object[]) msg.obj)[2];

                list.setAdapter(new PersonListAdapter(context, people));

                LinearLayout progressBar = (LinearLayout) findViewById(R.id.progress_container);
                progressBar.setVisibility(View.GONE);
            }
        }
    };



}
