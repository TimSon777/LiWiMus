import React, {useState, useEffect} from "react";
import {useParams} from "react-router-dom"
import {Button} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";

export default function UserProfilePage() {
    const {id} = useParams();
    const [userInfo, setUserInfo] = useState([]);
    
    useEffect(() => {
        (async () => {
            const res = await fetch('/users/'+id)
            const data = await res.json();
            setUserInfo(data);
        })();
        
    },[]);
    return (
        <div>
            <div>
                <h1>User Profile {id}</h1>
            </div>
        </div>
    );
}
