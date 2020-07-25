import React, { Component } from 'react';
import API from "../utils/API";

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            originalUrl: '',
            alias: '',
            errorMessages: []
        };
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.validateForm = this.validateForm.bind(this);
    }

    handleInputChange(event) {
        const value = event.target.value;
        const name = event.target.name;

        this.setState({ [name]: value });
    }

    validateForm() {
        this.setState({
            errorMessages: []
        });

        let messages = [];

        if (this.state.originalUrl.length == 0) {
            messages.push('You must enter a URL');
        }

        if (this.state.alias.length == 0) {
            messages.push('You must enter an alias');
        }

        this.setState({
            errorMessages: messages
        });

        return this.state.originalUrl.length > 0 ||
            this.state.alias.length > 0;
    }

    async handleSubmit(event) {
        event.preventDefault();

        let formIsValid = this.validateForm();

        if (formIsValid) {
            await API.post('/', { originalUrl: this.state.originalUrl, shortUrl: this.state.alias }).then(response => {
                this.props.history.push('/success/' + this.state.alias);
            })
            .catch(error => {
                var data = error.response.data;
                this.setState({
                    errorMessages: data,
                    formValid: false
                });
            });
        }
    }

  render () {
    return (
        <div className="main">
            <form>
                <h1>Free URL Shortener</h1>
                <div className="row">
                    <label htmlFor="originalUrl">URL To Shorten</label>
                    <input className="text-input" id="originalUrl" type="text" value={this.state.originalUrl} onChange={this.handleInputChange} name="originalUrl" />
                </div>
                <div className="row list">
                    <div className="input-container">
                        <label htmlFor="alias">Alias</label>
                        <input className="text-input" id="alias" type="text" value={this.state.alias} onChange={this.handleInputChange} name="alias" />
                    </div>
                    <button type="submit" className="btn-primary" onClick={this.handleSubmit}>
                        <div className="btn-image" />
                        <span className="btn-text">Shorten!</span>
                    </button>
                </div>
                {this.state.errorMessages.length > 0 &&
                    <div className="row">
                    <h3>Errors</h3>
                    {this.state.errorMessages.map((message) => {
                        return (<p className="error">{message}</p>)
                    })}
                    </div>
                }
            </form>
      </div>
    );
  }
}