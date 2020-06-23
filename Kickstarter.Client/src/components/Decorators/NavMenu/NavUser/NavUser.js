import React, { useState } from 'react'
import Avatar from '@material-ui/core/Avatar';
import { Link } from 'react-router-dom';
import { NavLink } from 'reactstrap';
import { FastLinksPopover } from "./FastLinksPopover";
import { useSelector } from "react-redux";
import userManager from '../../../../auth/userManager';


export const NavUser = (props) => {
    const [open, setOpen] = useState(false)
    const user = useSelector(state => state.oidc.user);

    const login = async (event) => {
        event.preventDefault();
        await userManager.signinRedirect();
    }

    console.log(user);  

    const LogInLink = () => {
        return(
            <NavLink className="link" tag={Link} to='/LogIn' onClick={login}>Log in</NavLink>
        )
    }

    const UserPicture = () => {
        return(
            <Avatar 
                alt={user.profile.name}
                src="/broken-image.jpg" 
                className="bg-green-main avatar"
                onClick={() => setOpen(true)}
            />
        )
    }


    const component = !user || user?.expired ? LogInLink() : UserPicture();

    return(
        <div>
            {component}
            <FastLinksPopover open={open} HandleClose={() => setOpen(false)}/>
        </div>
    )
}