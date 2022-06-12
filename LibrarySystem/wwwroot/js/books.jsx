class Book extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: props.book };
        this.onClick = this.onClick.bind(this);
    }

    onClick(e) {
        this.props.onRemove(this.state.data);
    }

    render() {
        return <tr>
            <td>{this.state.data.author}</td>
            <td>{this.state.data.title}</td>
            <td>{this.state.data.year}</td>
            <td>{this.state.data.genre}</td>
            <td className="text-center">
                <form action="Home/Delete" method="post">
                    <a href={"Home/Edit?bookId=" + this.state.data.id} className="btn btn-sm btn-warning me-2">Изменить</a>
                    <input type="hidden" name="bookId" value={this.state.data.id} />
                    <button type="submit" className="btn btn-sm btn-danger">Удалить</button>
                </form>
            </td>
        </tr>
    }
}

class BookForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { author: "", title: "", year: 0, genre: "" };

        this.onSubmit = this.onSubmit.bind(this);
        this.onAuthorChange = this.onAuthorChange.bind(this);
        this.onTitleChange = this.onTitleChange.bind(this);
        this.onYearChange = this.onYearChange.bind(this);
        this.onGenreChange = this.onGenreChange.bind(this);
    }
    onAuthorChange(e) {
        this.setState({author : e.target.value});
    }
    onTitleChange(e) {
        this.setState({ title: e.target.value });
    }
    onYearChange(e) {
        this.setState({ year: e.target.value });
    }
    onGenreChange(e) {
        this.setState({ genre: e.target.value });
    }
    onSubmit(e) {
        e.preventDefault();
        var bookAuthor = this.state.author.trim();
        var bookTitle = this.state.title.trim();
        var bookYear = this.state.year;
        var bookGenre = this.state.genre.trim();

        if (!bookAuthor || !bookTitle || !bookGenre || bookYear <= 1) {
            return;
        }

        this.props.onBookSubmit({
            author: bookAuthor,
            title: bookTitle,
            year: bookYear,
            genre: bookGenre
        });

        this.setState({ author: "", title: "", year: 0, genre: "" });
    }

    render() {
        return (
            <div className="modal fade" id="modal" data-bs-backdrop="static" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="staticBackdropLabel">Добавить книгу</h5>
                        </div>
                        <form onSubmit={this.onSubmit}>
                        <div className="modal-body">
                            <div>
                                    <input className="form-control mt-2" type="text"
                                    placeholder="Автор"
                                    value={this.state.author}
                                    onChange={this.onAuthorChange} />
                            </div>
                            <div>
                                    <input className="form-control mt-2" type="text"
                                    placeholder="Название"
                                    value={this.state.title}
                                    onChange={this.onTitleChange} />
                            </div>
                            <div>
                                    <input className="form-control mt-2" type="number"
                                    placeholder="Год издания"
                                    value={this.state.year}
                                    onChange={this.onYearChange} />
                            </div>
                            <div>
                                    <input className="form-control mt-2" type="text"
                                    placeholder="Жанр"
                                    value={this.state.genre}
                                    onChange={this.onGenreChange} />
                            </div>
                        
                        </div>
                        <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Закрыть</button>
                        <button type="submit" className="btn btn-primary" data-bs-dismiss="modal">Сохранить</button>
                        </div>
                        </form>
                </div>
              </div>
            </div>
            );
    }
}

class BooksList extends React.Component {
    constructor(props) {
        super(props);
        this.state = { books: [] };

        this.onAddBook = this.onAddBook.bind(this);
        this.onRemoveBook = this.onRemoveBook.bind(this);
    }

    loadData() {
        var xhr = new XMLHttpRequest();
        xhr.open("get", this.props.url, true);
        xhr.onload = () => {
            var data = JSON.parse(xhr.responseText);
            this.setState({ books: data });
        }
        xhr.send();
    }

    componentDidMount() {
        this.loadData();
    }

    onAddBook(book) {
        if (book) {
            const data = new FormData();
            data.append("author", book.author);
            data.append("title", book.title);
            data.append("year", book.year);
            data.append("genre", book.genre);

            var xhr = new XMLHttpRequest();
            xhr.open("post", "Home/SaveBook", true);
            xhr.onload = () => {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }
            xhr.send(data);
        }
    }

    onRemoveBook(book) {
        if (book) {
            var url = this.props.url + "/" + book.id;

            var xhr = new XMLHttpRequest();
            xhr.open("delete", url, true);
            xhr.SendRequestHeader("Content-Type", "application/json");
            xhr.onload = function () {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send();
        }
    }

    render() {
        var remove = this.onRemoveBook;
        return <table className="table">

            <thead>
                <tr>
                    <th>Автор</th>
                    <th>Название</th>
                    <th>Год издания</th>
                    <th>Жанр</th>
                    <th></th>
                </tr>
            </thead>

            <tbody>
            {<BookForm onBookSubmit={this.onAddBook} />}
            {
                this.state.books.map(function (book) {
                    return <Book key={book.id} book={book} onRemove={remove} />
                })
            }
            </tbody>
        </table>
    }
}

ReactDOM.render(<BooksList url="/Home/GetBooks" />,
    document.getElementById("content"));