import React, { Component } from 'react'
import PropTypes from 'prop-types'

export default class CommentInput extends Component {
  static propTypes = {
    username: PropTypes.any,
    onUserNameInputBlur: PropTypes.func,
    onSubmit: PropTypes.func,
  }

  static defaultProps = { username: '' }

  constructor(props) {
    super(props)
    this.state = { username: props.username, content: props.content }
  }

  handleUsernameBlur(e) {
    console.log(e.target)
    if (this.props.onUsernameInputBlur) this.props.onUsernameInputBlur(e.target.value)
  }

  handleContentBlur(e) {
    if (this.props.onContentInputBlur) this.props.onContentInputBlur(e.target.value)
  }

  handleUsernameChange(e) {
    this.setState({ username: e.target.value })
  }

  handleContentChange(e) {
    this.setState({ content: e.target.value })
  }

  handleSubmit(e) {
    if (this.props.onSubmit) {
      this.props.onSubmit({
        username: this.state.username,
        content: this.state.content,
        createdTime: +new Date()
      })
    }
    this.setState({ content: '' })
  }

  render() {
    return (
      <div className='comment-input'>
        <div className='comment-field'>
          <span className='comment-field-name'>用户名：</span>
          <div className='comment-field-input'>
            <input value={this.state.username} onChange={this.handleUsernameChange.bind(this)} onBlur={this.handleUsernameBlur.bind(this)} />
          </div>
        </div>
        <div className='comment-field'>
          <span className='comment-field-name'>评论内容：</span>
          <div className='comment-field-input'>
            <textarea value={this.state.content} onChange={this.handleContentChange.bind(this)} onBlur={this.handleContentBlur.bind(this)} />
          </div>
        </div>
        <div className='comment-field-button'>
          <button onClick={this.handleSubmit.bind(this)}>
            发布评论
          </button>
        </div>
      </div>
    )
  }
}