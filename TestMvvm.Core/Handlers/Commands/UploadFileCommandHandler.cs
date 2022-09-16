using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Services;

namespace TestMvvm.Core.Handlers.Commands
{
    public class UploadFileCommand : IRequest<string>
    {
        public IFormFile File { get; }
        public UploadFileCommand(IFormFile file)
        {
            this.File = file;
        }

    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
    {

        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UploadFileCommandHandler> _logger;

        public UploadFileCommandHandler(ILogger<UploadFileCommandHandler> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var file = request.File;
            return await _fileStorageService.UploadAsync(file);
        }

    }
}
