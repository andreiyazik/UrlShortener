import React, { Component } from 'react';
import { withRouter } from "react-router-dom";
import API from "../utils/API";
import * as constants from "../constants"

export class Success extends Component {
    static displayName = Success.name;

    constructor(props) {
        super(props);
        this.state = {
            originalUrl: '',
            alias: ''
        };
    }

    async componentDidMount() {
        let pathname = this.props.location.pathname;
        let key = pathname.substring(pathname.lastIndexOf('/') + 1, pathname.length);
        if (key == '' || key == 'success') {
            this.props.history.push('/');
        }

        await this.populateUrlData(key);
    }

    async populateUrlData(key) {
        await API.get('/details/' + key).then(response => {
            let data = response.data;
            this.setState({ originalUrl: data.originalUrl, alias: constants.API_URL + data.shortUrl });
        })
            .catch(error => {
                this.props.history.push('/');
            });
    }

    render() {
        return (
            <div className="main">
                <h1>Success!</h1>
                <input className="text-input" id="alias" type="text" value={this.state.alias} name="alias" readOnly />
                <div className="row">
                    <b>Long URL: </b><span>{this.state.originalUrl}</span>
                </div>
            </div>
        );
    }
}