package com.mattandmikeandscott.richpersonleaderboard.adapters;

import android.app.Dialog;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.mattandmikeandscott.richpersonleaderboard.MainActivity;
import com.mattandmikeandscott.richpersonleaderboard.R;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;

import java.text.NumberFormat;
import java.util.List;

public class PersonListAdapter extends BaseAdapter {
    private Context context;
    private NumberFormat formatter = NumberFormat.getCurrencyInstance();

    List<Person> people;

    public PersonListAdapter(Context context, List<Person> people) {
        this.context = context;
        this.people = people;
;
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

        // Person avatar

        convertView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showPersonProfileDialog(currentPerson.getId());
            }
        });

        return convertView;
    }

    private void showPersonProfileDialog(final int personId) {
        Dialog dialog = new Dialog(context, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
        dialog.setCancelable(true);

        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        final RelativeLayout layout = (RelativeLayout) inflater.inflate(R.layout.person_profile_dialog, null);

        ((TextView) layout.findViewById(R.id.person_profile_header_text)).setText(String.valueOf(personId));

        dialog.setContentView(layout);
        dialog.show();
    }
}