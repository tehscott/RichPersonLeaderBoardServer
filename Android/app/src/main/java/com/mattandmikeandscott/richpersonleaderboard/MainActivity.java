package com.mattandmikeandscott.richpersonleaderboard;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.google.android.gms.auth.api.Auth;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.auth.api.signin.GoogleSignInResult;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import com.mattandmikeandscott.richpersonleaderboard.adapters.PersonListAdapter;
import com.mattandmikeandscott.richpersonleaderboard.domain.GetPeopleResponse;
import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.RankType;

import java.util.Hashtable;
import java.util.Map;

import static com.mattandmikeandscott.richpersonleaderboard.MainActivity.HandlerResult.PEOPLE_INFO_AQUIRED;

public class MainActivity extends ActionBarActivity implements ActionBar.TabListener, GoogleApiClient.OnConnectionFailedListener {
    private SectionsPagerAdapter mSectionsPagerAdapter;
    private int myId = 1; // TODO: Setup logging in and such

    private static int RC_SIGN_IN = 1000;

    private ViewPager mViewPager;

    private GoogleApiClient mGoogleApiClient;

    @Override
    public void onConnectionFailed(ConnectionResult connectionResult) {

    }

    public enum HandlerResult {
        PEOPLE_INFO_AQUIRED, PERSON_INFO_AQUIRED
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

        setupButtons();








        // Configure sign-in to request the user's ID, email address, and basic profile. ID and
        // basic profile are included in DEFAULT_SIGN_IN.
        GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail()
                .build();

        // Build a GoogleApiClient with access to GoogleSignIn.API and the options above.
        mGoogleApiClient = new GoogleApiClient.Builder(this)
                .enableAutoManage(this, this)
                .addApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .build();

        findViewById(R.id.sign_in_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                signIn();
            }
        });
    }

    private void signIn() {
        Intent signInIntent = Auth.GoogleSignInApi.getSignInIntent(mGoogleApiClient);
        startActivityForResult(signInIntent, RC_SIGN_IN);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
        if (requestCode == RC_SIGN_IN) {
            GoogleSignInResult result = Auth.GoogleSignInApi.getSignInResultFromIntent(data);
            handleSignInResult(result);
        }
    }

    private void handleSignInResult(GoogleSignInResult result) {
        Log.d(getString(R.string.app_short_name), "handleSignInResult:" + result.isSuccess());
        if (result.isSuccess()) {
            // Signed in successfully, show authenticated UI.
            GoogleSignInAccount acct = result.getSignInAccount();
            //mStatusTextView.setText(getString(R.string.signed_in_fmt, acct.getDisplayName()));
            //updateUI(true);
        } else {
            // Signed out, show unauthenticated UI.
            //updateUI(false);
        }
    }


















    private void setupButtons() {
        findViewById(R.id.find_me_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Toast.makeText(MainActivity.this, "Searching with hard coded ID of 1", Toast.LENGTH_SHORT).show();
                findMe();
            }
        });
    }

    private void findMe() {
        Map<String, Integer> parameters = new Hashtable<>();
        parameters.put("id", myId);
        parameters.put("range", 5);

        int currentPosition = mViewPager.getCurrentItem();
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

        View fragmentList = mViewPager.findViewWithTag(tag);
        MainFragment.refreshListAsync(getResources(), MainActivity.this, fragmentList, PeopleQueryType.Myself, rankType, parameters);
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

            if (msg.what == PEOPLE_INFO_AQUIRED.ordinal()) {
                GetPeopleResponse response = (GetPeopleResponse) msg.obj;

                response.getList().setAdapter(new PersonListAdapter(response.getContext(), response.getPeople(), response.getPeopleQueryType(), response.getRankType()));

                final SwipeRefreshLayout swipeLayout = (SwipeRefreshLayout) response.getList().getParent();
                swipeLayout.setRefreshing(false);
                swipeLayout.setEnabled(true);
            }
        }
    };
}