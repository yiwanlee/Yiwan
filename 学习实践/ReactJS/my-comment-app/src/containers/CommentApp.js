import React, { Component } from 'react'
import PropTypes from 'prop-types'
import CommentInput from './../components/CommentInput';
import CommentList from './../components/CommentList';

class CommentApp extends Component {
    static propTypes = {
        comments: PropTypes.array
    }
    static defaultProps = {
        comments: []
    }

    constructor(props) {
        super(props)
        this.state = {
            username: localStorage.getItem('username') || '',
            content: localStorage.getItem('content') || '',
        }
    }

    handleUsernameInputBlur(val) {
        this.setState({ username: val })
        localStorage.setItem('username', val)
    }

    handleContentInputBlur(val) {
        this.setState({ content: val })
        localStorage.setItem('content', val)
    }

    render() {
        return (
            <div>
                <CommentInput username={this.state.username} content={this.state.content} onUsernameInputBlur={this.handleUsernameInputBlur.bind(this)} onContentInputBlur={this.handleContentInputBlur.bind(this)} />
                <CommentList />
            </div>
        )
    }
}

export default CommentApp