package com.mattandmikeandscott.richpersonleaderboard;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.List;

public class PersonListAdapter extends BaseAdapter implements View.OnClickListener {
    private Context context;

    List<String[]> people;

    public PersonListAdapter(Context context, List<String[]> people) {
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
        String[] currentPerson = people.get(position);

        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        convertView = inflater.inflate(R.layout.list_item, null);

        // Gender icon

        // Person name
        TextView gameName = (TextView) convertView.findViewById(R.id.person_name_textview);
        gameName.setText(currentPerson[0]);

        // Person worth
        TextView gameInfo = (TextView) convertView.findViewById(R.id.person_worth_textview);
        gameInfo.setText(currentPerson[1]);

        // Person avatar

        return convertView;
    }

    @Override
    public void onClick(View v) {

    }
}