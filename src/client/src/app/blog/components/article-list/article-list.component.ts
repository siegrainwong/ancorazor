import { Component, OnInit } from '@angular/core';
import { ArticleParameters } from '../../models/article-parameters';
import { PostService } from '../../services/post.service';
import ArticleModel from '../../models/article-model';
import { Pagination } from 'src/app/shared/models/response-result';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.scss']
})
export class ArticleListComponent implements OnInit {

  parameter = new ArticleParameters({ orderBy: 'id desc', pageSize: 10, pageIndex: 0 })

  articles: ArticleModel[]
  pagiation: Pagination

  constructor(private postService: PostService) { }

  ngOnInit() {
    this.getPost()
  }

  async getPost() {
    let res = await this.postService.getPagedPosts(this.parameter)
    this.articles = res.data as ArticleModel[]
    this.pagiation = res.pagination
  }
}