package com.mattandmikeandscott.richpersonleaderboard.adapters;

import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Handler;
import android.os.Message;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.mattandmikeandscott.richpersonleaderboard.MainActivity;
import com.mattandmikeandscott.richpersonleaderboard.R;
import com.mattandmikeandscott.richpersonleaderboard.domain.PeopleQueryType;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;
import com.mattandmikeandscott.richpersonleaderboard.network.Repository;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;
import java.util.Map;

import static com.mattandmikeandscott.richpersonleaderboard.adapters.PersonListAdapter.HandlerResult.PERSON_INFO_AQUIRED;
import static com.mattandmikeandscott.richpersonleaderboard.adapters.PersonListAdapter.HandlerResult.PERSON_INFO_FAILED;

public class PersonListAdapter extends BaseAdapter {
    private Context context;
    private NumberFormat formatter = NumberFormat.getCurrencyInstance();
    private List<Person> people;
    public enum HandlerResult {
        PERSON_INFO_AQUIRED,
        PERSON_INFO_FAILED
    }

    public PersonListAdapter(Context context, List<Person> people) {
        this.context = context;
        this.people = people;
    }

    public int getCount() {
        return people.size();
    }

    public Object getItem(int position) {
        return people.get(position);
    }

    public long getItemId(int position) {
        return position;
    }

    public View getView(int position, View convertView, ViewGroup viewGroup) {
        final Person currentPerson = people.get(position);

        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        convertView = inflater.inflate(R.layout.list_item, null);

        if(position % 2 == 0) {
            convertView.setBackgroundResource(R.color.colorLightBlue);
        }

        // Rating
        TextView rank = (TextView) convertView.findViewById(R.id.rank_textview);
        rank.setText(String.valueOf(currentPerson.getRank()));

        // Person name
        TextView gameName = (TextView) convertView.findViewById(R.id.person_name_textview);
        gameName.setText(currentPerson.getName());

        // Person wealth
        TextView gameInfo = (TextView) convertView.findViewById(R.id.person_wealth_textview);
        gameInfo.setText(formatter.format(currentPerson.getWealth()));

        convertView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showPersonProfileDialog(currentPerson.getId());
            }
        });

        return convertView;
    }

    private void showPersonProfileDialog(final int personId) {
        final Dialog dialog = new Dialog(context, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
        dialog.setCancelable(true);

        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        final RelativeLayout layout = (RelativeLayout) inflater.inflate(R.layout.person_profile_dialog, null);

        layout.findViewById(R.id.profile_back_button).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialog.dismiss();
            }
        });

        dialog.setContentView(layout);

        dialog.setOnShowListener(new DialogInterface.OnShowListener() {
            @Override
            public void onShow(DialogInterface dialog) {
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        Repository repository = new Repository(context.getResources());

                        Map<String, Integer> parameters = new Hashtable<>();
                        parameters.put("id", personId);

                        ArrayList<Person> person = repository.getPeople(PeopleQueryType.Person, parameters);

                        if(person.size() > 0) {
                            Object[] messageParams = new Object[] { person.get(0), layout };

                            Message message = new Message();
                            message.what = PERSON_INFO_AQUIRED.ordinal();
                            message.obj = messageParams;

                            handler.sendMessage(message);
                        } else {
                            handler.sendEmptyMessage(PERSON_INFO_FAILED.ordinal());
                        }
                    }
                }).start();
            }
        });

        dialog.show();
    }

    public Handler handler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage(msg);

            if (msg.what == PERSON_INFO_FAILED.ordinal()) {
                Toast.makeText(context, "Failed to fetch the profile.", Toast.LENGTH_SHORT).show();
            } else if (msg.what == PERSON_INFO_AQUIRED.ordinal()) {
                Person person = (Person) ((Object[]) msg.obj)[0];
                RelativeLayout layout = (RelativeLayout) ((Object[]) msg.obj)[1];

                ((TextView) layout.findViewById(R.id.person_profile_header_text)).setText(String.valueOf(person.getName()));
                layout.findViewById(R.id.profile_progress_bar).setVisibility(View.GONE);
            }
        }
    };
}