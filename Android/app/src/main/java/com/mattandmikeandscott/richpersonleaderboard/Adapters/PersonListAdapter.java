package com.mattandmikeandscott.richpersonleaderboard.adapters;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.mattandmikeandscott.richpersonleaderboard.R;
import com.mattandmikeandscott.richpersonleaderboard.domain.Person;

import java.util.List;

public class PersonListAdapter extends BaseAdapter implements View.OnClickListener {
    private Context context;

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
        Person currentPerson = people.get(position);

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

        // Person worth
        TextView gameInfo = (TextView) convertView.findViewById(R.id.person_worth_textview);
        gameInfo.setText(String.valueOf(currentPerson.getNetWorth()));

        // Person avatar

        return convertView;
    }

    @Override
    public void onClick(View v) {

    }
}