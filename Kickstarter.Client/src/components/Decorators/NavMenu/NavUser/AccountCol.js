import React from 'react';
import { Link } from 'react-router-dom';

const SavedProjects = 'Saved projects';
const Profile = 'Profile';
const Settings = 'Settings';


export const AccountCol = (props) => {
    return(
        <ul className="pr6 pb6-sm pl4-sm pt05">
            <li className="p1 mb1 mb0-sm p0-sm">
                <Link to={"/"} className="link link-bigger">
                    <p>{Profile}</p>
                </Link>
            </li>
            <li className="p1 mb1 mb0-sm p0-sm">
                <Link to={"/"} className="link link-bigger">
                    <p>{Settings}</p>
                </Link>
            </li>
            <li className="p1 mb1 mb0-sm p0-sm">
                <Link to={"/"} className="link link-bigger">
                    <p>{SavedProjects}</p>
                </Link>
            </li>
        </ul>
    )
}